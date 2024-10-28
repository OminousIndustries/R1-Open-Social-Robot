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
    public string response_object { get; set; } // changed from 'response_object' to 'object'
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


public class RoboLogic : MonoBehaviour
{
    private LipSyncManager lipSyncManager;
    private RoboSpeak tts;
    private RoboListen stt;
    [SerializeField]
    private string API_KEY = ""; // Replace this with your OpenAI API Key

    [SerializeField]
    private string persona = ""; // Replace this with your persona
    private static string ResponseName;
    private bool myFunctionCalled;
    public static bool newDataReceived;
    private int receivedDataCount;
    private int processedDataCount;
    private bool firstTime = true; // Flag to check if the application has started for the first time
    public bool isPaused;
    // Add a new flag at the beginning of the class
    private bool resumeFlag = false;  // Added resume flag


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
            yield return StartCoroutine(CallOpenAIApi(initialMessage));
        }
    }



    public IEnumerator CallOpenAIApi(string message)
    {
        var API_URL = "https://api.openai.com/v1/chat/completions";

        string requestData = "{\"model\": \"gpt-3.5-turbo\", " +
                             "\"messages\": [" +
                             "{\"role\": \"system\", \"content\": \"" + persona + "\"}, " +
                             "{\"role\": \"user\", \"content\": \"" + message + "\"}" +
                             "]}";

        using (UnityWebRequest request = UnityWebRequest.Post(API_URL, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Authorization", "Bearer " + API_KEY);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(request.error);
            }
            else
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
                    Debug.Log("No choices in GPT response.");
                    yield break;
                }

                string gptResponseText = gptResponse.choices[0].message.content;  // Change to use the new 'message' structure
                gptResponseText = gptResponseText.Replace(",", "");
                Name = gptResponseText;
                receivedDataCount++;
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
            //Debug.Log("Paused");
            tts.StopCurrentAudio();  // Stop and clear the current audio when paused
        }

        // Check if the gamepad's "B" Button is pressed
        if (isPaused && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Resume();
            //Debug.Log("Resume");
        }

        if (isPaused)
        {
            return;
        }

        if (tts.audioSourceNeedStop && !myFunctionCalled)
        {
            stt.ButtonClick();
            //Debug.Log("Listening");
            myFunctionCalled = true;
        }

        if (receivedDataCount > processedDataCount)
        {
            newDataReceived = true;
            if (newDataReceived)
            {
                //Debug.Log("New data received, name: " + Name);  // Logging the new data received
                tts.ButtonClick();
                lipSyncManager.SetInputAndPlay(Name);

                //Debug.Log("Speaking");
                processedDataCount++;
                myFunctionCalled = false;
            }
        }
    }


}