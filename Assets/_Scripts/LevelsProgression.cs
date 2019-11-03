using System;
using UnityEngine;

public class LevelsProgression : MonoBehaviour
{
    public event Action<int> CurrentLevelChanged;
    public event Action<int> MaximumLevelChanged;

    public int CurrentLevel
    {
        get { return PlayerPrefs.GetInt("CurrentLevel", 0); }
        set
        {
            PlayerPrefs.SetInt("CurrentLevel", value);
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

    public void OpenNextLevel()
    {
        MaximumLevel++;
    }
}
