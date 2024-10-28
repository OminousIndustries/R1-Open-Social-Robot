using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public class GptResponse
{
    public string id { get; set; }
    public string response_object { get; set; }
    public string created { get; set; }
    public string model { get; set; }
    public List<Choice> choices { get; set; }
}

public class Choice
{
    public Message message { get; set; }
    public string finish_reason { get; set; }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

[System.Serializable]
public class MessageRequestData
{
    public Message[] messages;
    public string mode = "chat";
    public string character;
}

public class RoboLogic : MonoBehaviour
{
    private LipSyncManager lipSyncManager;
    private RoboSpeak tts;
    private RoboListen stt;

    [SerializeField]
    private string characterName = "Name"; // Change this to the character name you want to use. This is under Parameters -> Chat -> Character in the webui

    [SerializeField]
    private string localEndpoint = "http://192.168.50.125:5000/v1/chat/completions"; // Change the ip to the address of the machine running your webui

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
        // Create a temporary object to match the expected format
        var tempMessages = new Message[]
        {
            new Message { role = "user", content = message }
        };

        // Create the request object
        var requestObject = new
        {
            messages = tempMessages,
            mode = "chat",
            character = characterName
        };

        // Serialize using Newtonsoft.Json instead of JsonUtility
        string jsonData = JsonConvert.SerializeObject(requestObject);
        Debug.Log($"Sending request data: {jsonData}"); // Debug the request data

        using (UnityWebRequest request = new UnityWebRequest(localEndpoint, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseBody = request.downloadHandler.text;
                Debug.Log("Received response body: " + responseBody);

                if (string.IsNullOrEmpty(responseBody))
                {
                    Debug.Log("Response body is null or empty.");
                    yield break;
                }

                GptResponse gptResponse;
                try
                {
                    gptResponse = JsonConvert.DeserializeObject<GptResponse>(responseBody);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to parse response body: " + e.Message);
                    yield break;
                }

                if (gptResponse == null || gptResponse.choices == null || gptResponse.choices.Count == 0)
                {
                    Debug.Log("No choices in response.");
                    yield break;
                }

                string responseText = gptResponse.choices[0].message.content;
                responseText = responseText.Replace(",", "");
                Name = responseText;
                receivedDataCount++;
            }
            else
            {
                Debug.LogError($"Error Details:");
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