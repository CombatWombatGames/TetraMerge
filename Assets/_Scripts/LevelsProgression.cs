using System;
using UnityEngine;

public class LevelsProgression : MonoBehaviour
{
    public int CurrentLevel
    {
        get { return PlayerPrefs.GetInt("LastLevel", 0); }
        set
        {
            PlayerPrefs.SetInt("LastLevel", value);
            CurrentLevelChanged(value);
        }
    }

    public int MaximumLevel
    {
        get { return PlayerPrefs.GetInt("MaximumLevel", 0); }
        set
        {
            PlayerPrefs.SetInt("MaximumLevel", value);
            MaximumLevelChanged(value);
        }
    }

    public event Action<int> CurrentLevelChanged = delegate { };
    public event Action<int> MaximumLevelChanged = delegate { };

    public void OpenNextLevel()
    {
        MaximumLevel++;
    }
}
