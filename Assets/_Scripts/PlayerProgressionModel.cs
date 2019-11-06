using System;
using UnityEngine;

//Counts current score and holds best score
public class PlayerProgressionModel : MonoBehaviour
{
    public event Action<int> TurnChanged;
    public event Action<int> CurrentScoreChanged;
    public event Action<int> BestScoreChanged;
    public event Action<int> LevelNumberChanged;

    public int CurrentScore { get; private set; }

    public int TurnNumber
    {
        get { return turnNumber; }
        set
        {
            turnNumber = value;
            TurnChanged(value);
        }
    }

    public int BestScore
    {
        get { return PlayerPrefs.GetInt("BestScore", 0); }
        set
        {
            PlayerPrefs.SetInt("BestScore", value);
            BestScoreChanged(value);
        }
    }

    public int LevelNumber
    {
        get { return levelNumber; }
        set
        {
            levelNumber = value;
            LevelNumberChanged(value);
        }
    }

    [SerializeField] GridModel gridModel = default;

    int turnNumber;
    int levelNumber;

    void Awake()
    {
        gridModel.GridChanged += OnGridChanged;
    }

    void OnDestroy()
    {
        gridModel.GridChanged -= OnGridChanged;
    }


    void Start()
    {
        TurnNumber = 0;
        LevelNumber = 1;
    }

    void OnGridChanged(Vector2Int[] area, int level)
    {
        CurrentScore = 0;
        for (int i = 0; i < gridModel.Grid.GetLength(0); i++)
        {
            for (int j = 0; j < gridModel.Grid.GetLength(1); j++)
            {
                if (gridModel.Grid[i, j].Level != 0)
                {
                    CurrentScore += gridModel.Grid[i, j].Level;
                    //Old way: always grows, but depends on level too much
                    //CurrentScore += Mathf.RoundToInt(Mathf.Pow(10, gridModel.Grid[i, j].Level - 1));
                }
            }
        }
        CurrentScoreChanged(CurrentScore);
    }
}