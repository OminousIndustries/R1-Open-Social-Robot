using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System.Collections.Concurrent;
using System;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif

public class RoboListen : MonoBehaviour
{
    private object threadLocker = new object();
    public bool waitingForReco;
    public string message;
    public RoboLogic roboLogicInstance;
    private bool micPermissionGranted = false;

    [SerializeField]
    private string SubscriptionKey = "";

    [SerializeField]
    private string Region = "";

    private readonly ConcurrentQueue<Action> _mainThreadWorkQueue = new ConcurrentQueue<Action>();

#if PLATFORM_ANDROID || PLATFORM_IOS
    private Microphone mic;
#endif

    private void Awake()
    {
        roboLogicInstance = transform.GetComponent<RoboLogic>();
    }

    public async void ButtonClick()
    {
        var config = SpeechConfig.FromSubscription(SubscriptionKey, Region);

        using (var recognizer = new SpeechRecognizer(config))
        {
            lock (threadLocker)
            {
                waitingForReco = true;
            }

            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            string newMessage = string.Empty;
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                newMessage = result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                newMessage = "NOMATCH: Speech could not be recognized.";
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
            }

            lock (threadLocker)
            {
                message = newMessage;
                Debug.Log(message);
                // Changed to use the new method name
                _mainThreadWorkQueue.Enqueue(() => StartCoroutine(roboLogicInstance.CallLocalLLM(message)));
                Debug.Log("Sent message to Local LLM");
                waitingForReco = false;
            }
        }
    }

    void Start()
    {
#if PLATFORM_ANDROID
        message = "Waiting for mic permission";
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#elif PLATFORM_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }
#else
        micPermissionGranted = true;
        message = "Click button to recognize speech";
#endif
    }

    void Update()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#elif PLATFORM_IOS
        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#endif

        while (_mainThreadWorkQueue.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }
}