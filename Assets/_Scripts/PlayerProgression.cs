using System;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    public event Action<int> LevelChanged;
    public event Action<int> BestScoreChanged;

    public int CurrentLevel
    {
        get { return PlayerPrefs.GetInt("CurrentLevel", 0); }
        set
        {
            PlayerPrefs.SetInt("CurrentLevel", value);
            LevelChanged(value);
        }
    }

    //10 ^ (level - 1) for each tile with level > 0
    public int BestScore
    {
        get { return PlayerPrefs.GetInt("BestScore", 0); }
        set
        {
            PlayerPrefs.SetInt("BestScore", value);
            BestScoreChanged(value);
        }
    }
}
