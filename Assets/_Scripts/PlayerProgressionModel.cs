using System;
using UnityEngine;

//Counts current score and holds best score
public class PlayerProgressionModel : MonoBehaviour
{
    public event Action<int> TurnChanged;
    public event Action<int> CurrentScoreChanged;
    public event Action<int> BestScoreChanged;
    public event Action<int> LevelNumberChanged;

    public int CurrentScore
    {
        get { return currentScore; }
        private set
        {
            currentScore = value;
            CurrentScoreChanged(value);
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
        get { return bestScore; }
        set
        {
            bestScore = value;
            BestScoreChanged(value);
        }
    }

    public int BestLevel
    {
        get { return bestLevel; }
        set
        {
            bestLevel = value;
        }
    }

    GridModel gridModel;
    int currentScore;
    int levelNumber;
    int turnNumber;
    int bestScore;
    int bestLevel;

    void Awake()
    {
        gridModel = GetComponent<GridModel>();
        gridModel.GridChanged += OnGridChanged;
        LevelNumberChanged += UpdateBestLevel;
    }

    void OnDestroy()
    {
        gridModel.GridChanged -= OnGridChanged;
        LevelNumberChanged -= UpdateBestLevel;
    }

    public void Initialize(int currentScore, int levelNumber, int turnNumber, int bestScore, int bestLevel)
    {
        CurrentScore = currentScore;
        LevelNumber = levelNumber;
        TurnNumber = turnNumber;
        BestScore = bestScore;
        BestLevel = bestLevel;
    }

    void OnGridChanged(Vector2Int[] area, int level)
    {
        if (level == 0)
        {
            int score = 0;
            if (area.Length > 25)
            {
                score = 100;
            }
            else if (area.Length > 16)
            {
                score = 50;
            }
            else if (area.Length > 9)
            {
                score = 25;
            }
            else if (area.Length > 4)
            {
                score = 10;
            }
            else if (area.Length > 1)
            {
                score = 5;
            }
            CurrentScore += score;
        }
    }

    public void UpdateBestScore()
    {
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }
    }

    private void UpdateBestLevel(int obj)
    {
        if (levelNumber > BestLevel)
        {
            BestLevel = levelNumber;
        }
    }
}