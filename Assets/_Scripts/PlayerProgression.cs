using System;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    public event Action<int> CurrentScoreChanged;
    public event Action<int> BestScoreChanged;

    //10 ^ (level - 1) for each tile with level > 0
    public int CurrentScore { get; private set; }

    public int BestScore
    {
        get { return PlayerPrefs.GetInt("BestScore", 0); }
        set
        {
            PlayerPrefs.SetInt("BestScore", value);
            BestScoreChanged(value);
        }
    }

    [SerializeField] GridModel gridModel = default;

    void Awake()
    {
        gridModel.GridChanged += OnGridChanged;
    }

    void OnDestroy()
    {
        gridModel.GridChanged -= OnGridChanged;
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
                    CurrentScore += Mathf.RoundToInt(Mathf.Pow(10, gridModel.Grid[i, j].Level - 1));
                }
            }
        }
        CurrentScoreChanged(CurrentScore);
    }
}