using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

// Old OpenAI-compatible data structures - Commented out
//public class GptResponse
//{
//    public string id { get; set; }
//    public string response_object { get; set; }
//    public string created { get; set; }
//    public string model { get; set; }
//    public List<Choice> choices { get; set; }
//}

//public class Choice
//{
//    public Message message { get; set; }
//    public string finish_reason { get; set; }
//}

//public class Message
//{
//    public string role { get; set; }
//    public string content { get; set; }
//}

// New Gemini API Data Structures
[System.Serializable]
public class GeminiRequest
{
    public Content[] contents;
    // Optional: public GenerationConfig generationConfig;
    // Optional: public SafetySetting[] safetySettings;
}

[System.Serializable]
public class Content
{
    public Part[] parts;
    public string role; // "user" for requests, "model" for responses
}

[System.Serializable]
public class Part
{
    public string text;
}

[System.Serializable]
public class GeminiResponse
{
    public Candidate[] candidates;
    public PromptFeedback promptFeedback;
}

[System.Serializable]
public class Candidate
{
    public Content content; // Reuses the Content class defined above
    public string finishReason;
    public int index;
    public SafetyRating[] safetyRatings;
}

[System.Serializable]
public class SafetyRating
{
    public string category;
    public string probability;
}

[System.Serializable]
public class PromptFeedback
{
    public SafetyRating[] safetyRatings;
}

public class RoboLogic : MonoBehaviour
{
    private LipSyncManager lipSyncManager;
    private RoboSpeak tts;
    private RoboListen stt;

    // [SerializeField]
    // private string characterName = "Name"; // Change this to the character name you want to use. This is under Parameters -> Chat -> Character in the webui

    // [SerializeField]
    // private string localEndpoint = "http://192.168.50.125:5000/v1/chat/completions"; // Change the ip to the address of the machine running your webui

    [SerializeField]
    private string geminiApiKey = ""; // TODO: Add your Gemini API Key here

    [SerializeField]
    private string geminiModelName = "gemini-1.5-flash-latest";

    private static string ResponseName;
    private bool myFunctionCalled;
    public static bool newDataReceived;
    private int receivedDataCount;
    private int processedDataCount;
    private bool firstTime = true;
    public bool isPaused;
    private bool resumeFlag = false;

    public static string Name
    {
        get { return ResponseName; }
        set { ResponseName = value; }
    }

    private void Awake()
    {
        lipSyncManager = transform.GetComponent<LipSyncManager>();
        tts = transform.GetComponent<RoboSpeak>();
        stt = transform.GetComponent<RoboListen>();
    }

    private void Start()
    {
        StartCoroutine(SendInitialMessage());
    }

    private IEnumerator SendInitialMessage()
    {
        if (firstTime || resumeFlag)
        {
            string initialMessage = "Hello";
            firstTime = false;
            resumeFlag = false;
            yield return StartCoroutine(CallLocalLLM(initialMessage));
        }
    }

    // Keep this method for backward compatibility
    public IEnumerator CallOpenAIApi(string message)
    {
        yield return StartCoroutine(CallLocalLLM(message));
    }

    public IEnumerator CallLocalLLM(string message)
    {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{geminiModelName}:generateContent?key={geminiApiKey}";

        // Create the request object using Gemini API structure
        var requestObject = new GeminiRequest
        {
            contents = new Content[]
            {
                new Content
                {
                    role = "user", // Gemini API uses "user" for messages from the user
                    parts = new Part[] { new Part { text = message } }
                }
            }
        };

        string jsonData = JsonConvert.SerializeObject(requestObject);
        Debug.Log($"Sending Gemini request data: {jsonData}");

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            // The API key is part of the URL, so no "Authorization" header is typically needed for Gemini's generateContent endpoint.

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseBody = request.downloadHandler.text;
                Debug.Log("Received Gemini response body: " + responseBody);

                if (string.IsNullOrEmpty(responseBody))
                {
                    Debug.LogError("Gemini API response body is null or empty.");
                    yield break;
                }

                GeminiResponse geminiResponse;
                try
                {
                    geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(responseBody);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to parse Gemini API response body: " + e.Message + "\nResponse Body: " + responseBody);
                    yield break;
                }

                if (geminiResponse == null)
                {
                    Debug.LogError("Parsed GeminiResponse is null.");
                    yield break;
                }
                
                if (geminiResponse.candidates == null || geminiResponse.candidates.Length == 0)
                {
                    Debug.LogWarning("No candidates in Gemini response.");
                    // Check for prompt feedback if candidates are missing, as it might contain blocking reasons
                    if (geminiResponse.promptFeedback != null && geminiResponse.promptFeedback.safetyRatings != null && geminiResponse.promptFeedback.safetyRatings.Length > 0)
                    {
                        foreach (var feedbackRating in geminiResponse.promptFeedback.safetyRatings)
                        {
                            Debug.LogWarning($"Prompt Feedback Safety Rating: Category: {feedbackRating.category}, Probability: {feedbackRating.probability}");
                        }
                    }
                    yield break;
                }

                var firstCandidate = geminiResponse.candidates[0];
                if (firstCandidate.content == null || firstCandidate.content.parts == null || firstCandidate.content.parts.Length == 0)
                {
                    Debug.LogWarning("No content or parts in the first candidate of Gemini response.");
                     if (firstCandidate.finishReason != null) {
                        Debug.LogWarning($"Candidate finishReason: {firstCandidate.finishReason}");
                    }
                    yield break;
                }

                string responseText = firstCandidate.content.parts[0].text;
                // responseText = responseText.Replace(",", ""); // This line might be specific to old API's output, evaluate if needed for Gemini
                Name = responseText;
                receivedDataCount++;
            }
            else
            {
                Debug.LogError($"Gemini API Error Details:");
                Debug.LogError($"Error: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");

                var responseHeaders = request.GetResponseHeaders();
                if (responseHeaders != null)
                {
                    foreach (var header in responseHeaders)
                    {
                        Debug.LogError($"Header - {header.Key}: {header.Value}");
                    }
                }

                if (request.downloadHandler != null && !string.IsNullOrEmpty(request.downloadHandler.text))
                {
                    Debug.LogError($"Response Body: {request.downloadHandler.text}");
                }
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
        resumeFlag = true;
        StartCoroutine(SendInitialMessage());
    }

    void Update()
    {
        if (!isPaused && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Pause();
            tts.StopCurrentAudio();
        }

        if (isPaused && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Resume();
        }

        if (isPaused)
        {
            return;
        }

        if (tts.audioSourceNeedStop && !myFunctionCalled)
        {
            stt.ButtonClick();
            myFunctionCalled = true;
        }

        if (receivedDataCount > processedDataCount)
        {
            newDataReceived = true;
            if (newDataReceived)
            {
                tts.ButtonClick();
                lipSyncManager.SetInputAndPlay(Name);
                processedDataCount++;
                myFunctionCalled = false;
            }
        }
    }
}