using System.Collections.Generic;

public class ConversionDictionary
{
    public Dictionary<string, List<string>> Dictionary;

    public ConversionDictionary()
    {
        Dictionary = new Dictionary<string, List<string>>();
    }

    public void Add(string key, List<string> value)
    {
        Dictionary.Add(key, value);
    }

    public List<string> GetKeysWhichStartsWith(string startKey)
    {
        List<string> result = new List<string>();
        foreach (string key in Dictionary.Keys)
        {
            if (key.StartsWith(startKey))
            {
                result.Add(key);
            }
        }
        return result;
    }

    public List<string> GetConversion(string key)
    {
        return Dictionary[key];
    }
}
