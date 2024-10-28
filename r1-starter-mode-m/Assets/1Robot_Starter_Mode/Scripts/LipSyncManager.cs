using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;



[Serializable]
public struct SpriteMap
{
    // It contains which letters have the same sprites.
    public string[] Characters;
    public Sprite Sprite;
}

public class LipSyncManager : MonoBehaviour
{
    [Tooltip("This is a trigger. If you check it, the character will put the currently Input parameter into the input queue. You can use it to test through the Inspector window.")]
    public bool PutInputIntoQueue;
    [TextArea]
    public string Input;

    [Tooltip("Reduce it to speed up the animation or increase it to slow down.")]
    public float WaitTimeBetweenCharacters;

    [Tooltip("It will wait this amount on every space character in the input string. Reduce it to speed up the animation or increase it to slow down.")]
    public float WaitTimeBetweenWords;

    [Tooltip("It recognises these staves at the end of the sentences: ' ! ? .'")]
    public float WaitTimeBetweenSentences;
    public bool UseSmartCorrect;

    private ConversionDictionary _conversionDictionary;
    private Image _imageComponent;
    private Queue<string> _inputQueue = new Queue<string>();
    private bool _duringAnimation;
    //private RoboSpeak tts;

    [Tooltip("Contains all letters and the sprites for each of them.")]
    public SpriteMap[] LetterSpriteList; // Remove the "[HideInInspector]" tag to make it visible in the Inspector window. 
    public Sprite IdleSprite; // Remove the "[HideInInspector]" tag to make it visible in the Inspector window.    
    public AudioSource audioSource; // Drag and drop the AudioSource component in the Inspector.

    //private void Awake()
    //{
    //    tts = transform.GetComponent<RoboSpeak>();

    //}

    private void Start()
    {
        _imageComponent = transform.Find("Character").GetComponent<Image>();

        CheckForAllLetters();
        CheckForDuplicates();
        AddConversionElements();

        // TODO: These are test inputs. Remove these lines!

        StartCoroutine("ReadInput");
    }

    private void Update()
    {
        if (PutInputIntoQueue)
        {
            PutInputIntoQueue = false;
            _inputQueue.Enqueue(Input);

            if (!_duringAnimation)
            {
                //tts.ButtonClick();
                StartCoroutine("ReadInput");
            }
        }
    }

    public void SetInputAndPlay(string input)
    {
        // Call this function from another class with the wanted string to start the animation.
        // Example: SetInputAndPlay("This is a long sentence which I want to animate. I don't have to do anything, only add it as a parameter");
        Input = input;
        _inputQueue.Enqueue(input);

        if (!_duringAnimation)
        {
            StartCoroutine("ReadInput");
        }
    }

    public void SetInputAndPlay(string input, float waitTimeBetweenCharacters, float waitTimeBetweenWords, float waitTimeBetweenSentences)
    {
        // Call this function from another class with the wanted string and change the elapsed times as well.
        // Example: SetInputAndPlay("This is a short sencence", 0.1f, 0.15f, 0.5f);
        Input = input;
        _inputQueue.Enqueue(input);
        WaitTimeBetweenCharacters = waitTimeBetweenCharacters;
        WaitTimeBetweenWords = waitTimeBetweenWords;
        WaitTimeBetweenSentences = waitTimeBetweenSentences;

        if (!_duringAnimation)
        {
            StartCoroutine("ReadInput");
        }
    }

    private void CheckForDuplicates()
    {
        // Check for duplicated characters in LetterSpriteList. If you put your own letters into the list, make sure that every letter has it's own sprite, and
        // there are no duplication, because it will cause error during the animation. 
        List<string> duplicateCheckList = new List<string>();

        foreach (SpriteMap map in LetterSpriteList)
        {
            foreach (String letter in map.Characters)
            {
                if (duplicateCheckList.Contains(letter))
                {
                    throw new Exception("Duplicated letter found in LetterSpriteList: " + letter);
                }
                else
                {
                    duplicateCheckList.Add(letter);
                }
            }
        }
    }

    private void CheckForAllLetters()
    {
        // Check all these letters for being present in the characters of LetterSpriteList, because if not, there will be an error during the animation.
        string[] letters = {
            "a", "b", "c", "ch", "d", "e", "ee", "f", "g",
            "i", "j", "k", "l", "m", "n", "o", "p",
            "q", "r", "s", "sh", "t", "th",
            "u", "v", "w", "x", "y", "z"};

        bool result = false;
        foreach (string letter in letters)
        {
            result = false;
            foreach (SpriteMap map in LetterSpriteList)
            {
                if (map.Characters.Contains(letter))
                {
                    result = true;
                }
            }

            if (!result)
            {
                throw new Exception("Cannot find letter '" + letter + "' in the list! Make sure that every letters are in the list!");
            }
        }
    }

    private void AddConversionElements()
    {
        // Add more elements if you want to.
        // It will replace part of the input with a list of another characters.
        // Make sure that you have all the images for every elements in the replaced characters and they are in the LetterSpriteList !!!
        _conversionDictionary = new ConversionDictionary();
        _conversionDictionary.Add("nk", new List<string> { "k" }); // Usually the 'n' and 'k' characters has the same sprite, so it is more sightful to present only one of them.
        _conversionDictionary.Add("dzs", new List<string> { "ch" }); // It is a hungarian letter. It's similar to the english 'ch'.
        _conversionDictionary.Add("oo", new List<string> { "o", "o", "o", "o" }); // It will be more sightful if there are more 'o' character, for example in 'good' word.
        _conversionDictionary.Add("ph", new List<string> { "f" });
        _conversionDictionary.Add("es", new List<string> { "s" });
    }

    private IEnumerator ReadInput()
    {
        // Get every element from input queue, set them for be suitable for animation (apply smart correction, if needed)
        _duringAnimation = true;
        while (_inputQueue.Count > 0)
        {
            string currentInput = _inputQueue.Dequeue();
            string[] letters = SeparateToLetters(currentInput);

            //Debug.Log("INPUT LETTERS: " + string.Join(",",letters.Select(x => x.ToString()).ToArray()));
            if (UseSmartCorrect)
            {
                letters = ConvertInput(letters);
                //Debug.Log("CONVERTED LETTERS: " + string.Join(",",letters.Select(x => x.ToString()).ToArray()));
            }

            foreach (string letter in letters)
            {
                if (!audioSource.isPlaying)
                {
                    break; // Exit the loop immediately if the audio source stops playing.
                }
                else if (letter.Equals(" "))
                {
                    _imageComponent.sprite = IdleSprite;
                    yield return new WaitForSeconds(WaitTimeBetweenWords);
                }
                else if (letter.Equals("?") || letter.Equals("!") || letter.Equals("."))
                {
                    _imageComponent.sprite = IdleSprite;
                    yield return new WaitForSeconds(WaitTimeBetweenSentences);
                }
                else
                {
                    Sprite sprite = GetSpriteBasedOnLetter(letter);
                    if (sprite != null)
                    {
                        _imageComponent.sprite = sprite;
                    }
                    else
                    {
                        throw new Exception("Cannot find sprite for letter '" + letter + "'!");
                    }
                    yield return new WaitForSeconds(WaitTimeBetweenCharacters);
                }
            }

            _imageComponent.sprite = IdleSprite;
        }
        _duringAnimation = false;
    }

    public bool IsDuringAnimation()
    {
        return _duringAnimation;
    }

    private string[] SeparateToLetters(string input)
    {
        // Separate the given input string into letters, spaces and staves
        List<string> result = new List<string>();
        Regex rgx = new Regex("[^a-zA-Z ?!.]");
        input = rgx.Replace(input, ""); // Remove all unrecognised characters.
        char[] characters = input.ToCharArray(); // Convert to character array.
        characters = Array.ConvertAll(characters, l => Char.ToLower(l)); // Convert every character to lowercase. 

        // Get the characters which are suitable for the animation.
        for (int i = 0; i < characters.Length; ++i)
        {
            switch (characters[i])
            {
                case 'a':
                case 'b':
                case 'd':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case ' ':
                case '?':
                case '!':
                case '.':
                    result.Add(characters[i].ToString());
                    break;
                case 't':
                    if (i < characters.Length - 1 && characters[i + 1].Equals('h'))
                    {
                        result.Add("th");
                        ++i;
                    }
                    else
                    {
                        result.Add(characters[i].ToString());
                    }
                    break;
                case 'c':
                    if (i < characters.Length - 1 && characters[i + 1].Equals('h'))
                    {
                        result.Add("ch");
                        ++i;
                    }
                    else
                    {
                        result.Add(characters[i].ToString());
                    }
                    break;
                case 's':
                    if (i < characters.Length - 1 && characters[i + 1].Equals('h'))
                    {
                        result.Add("sh");
                        ++i;
                    }
                    else
                    {
                        result.Add(characters[i].ToString());
                    }
                    break;
                case 'e':
                    if (i < characters.Length - 1 && characters[i + 1].Equals('e'))
                    {
                        result.Add("ee");
                        ++i;
                    }
                    else
                    {
                        result.Add(characters[i].ToString());
                    }
                    break;
                default:
                    break;
            }
        }

        return result.ToArray();
    }

    private string[] ConvertInput(string[] input)
    {
        // Convert the given array of strings into another based on the conversation dictionary
        List<string> result = new List<string>();
        int needToRemoveCharacterCount = 0;
        for (int i = 0; i < input.Length; ++i)
        {
            List<string> conversions = _conversionDictionary.GetKeysWhichStartsWith(input[i]);
            if (conversions.Count > 0)
            {
                bool foundConversion = false;
                if (conversions.Count == 1)
                {
                    if (conversions[0].Equals(input[i]))
                    {
                        result.AddRange(_conversionDictionary.GetConversion(input[i]));
                    }
                    else
                    {
                        string[] subInput = GetSubArray(input, i, input.Length - i);
                        string resolutionOfConversion = GetConvertedString(input[i], conversions, subInput, ref needToRemoveCharacterCount, ref foundConversion);
                        if (foundConversion)
                        {
                            result.AddRange(_conversionDictionary.GetConversion(resolutionOfConversion));
                        }
                        else
                        {
                            result.Add(input[i]);
                        }
                    }
                }
                else
                {
                    string[] subInput = GetSubArray(input, i, input.Length - i);
                    string resolutionOfConversion = GetConvertedString(input[i], conversions, subInput, ref needToRemoveCharacterCount, ref foundConversion);
                    if (foundConversion)
                    {
                        result.AddRange(_conversionDictionary.GetConversion(resolutionOfConversion));
                    }
                    else
                    {
                        result.Add(input[i]);
                    }
                }
            }
            else
            {
                result.Add(input[i]);
            }

            i += needToRemoveCharacterCount;
            needToRemoveCharacterCount = 0;
        }
        return result.ToArray();
    }

    private string GetConvertedString(string startString, List<string> foundKeys, string[] remainedInput, ref int iterationCount, ref bool foundConversion)
    {
        // Get the converted string from the conversation dictionary to the current start string
        string currentSearchedLetter = startString;
        if (remainedInput.Length > 1)
        {
            currentSearchedLetter += remainedInput[startString.Length];
        }

        int count = CountStringStartsWith(currentSearchedLetter, foundKeys);
        if (count == 0)
        {
            foundConversion = false;
            iterationCount = 0;
            return startString;
        }
        else if (count == 1 && foundKeys.Contains(currentSearchedLetter))
        {
            foundConversion = true;
            ++iterationCount;
            return currentSearchedLetter;
        }
        else
        {
            if (currentSearchedLetter.Length == 1)
            {
                return startString;
            }
            else
            {
                foundConversion = true;
                ++iterationCount;
                return GetConvertedString(currentSearchedLetter, foundKeys, remainedInput, ref iterationCount, ref foundConversion);
            }
        }
    }

    private Sprite GetSpriteBasedOnLetter(string letter)
    {
        // Get the character sprite image from the given letter
        foreach (SpriteMap map in LetterSpriteList)
        {
            if (Array.IndexOf(map.Characters, letter) != -1)
            {
                return map.Sprite;
            }
        }

        throw new Exception("Cannot find letter '" + letter + "' in the list!");
    }

    private string[] GetSubArray(string[] data, int startIndex, int length)
    {
        string[] result = new string[length];
        Array.Copy(data, startIndex, result, 0, length);
        return result;
    }

    private int CountStringStartsWith(string key, List<string> list)
    {
        // Count how many strings are in the given list which starts with the given key.
        int result = 0;
        foreach (string element in list)
        {
            if (element.StartsWith(key))
            {
                ++result;
            }
        }
        return result;
    }
}