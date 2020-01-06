using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

//TODO HIGH Loading (auto from start)
public class SaveSystem : MonoBehaviour
{
    public StateData stateData { get; private set; }

    [SerializeField] protected GridModel gridModel = default;
    [SerializeField] protected PlayerProgressionModel playerProgressionModel = default;
    [SerializeField] protected BoostersModel boostersModel = default;
    [SerializeField] protected PiecesModel piecesModel = default;
    protected string fileName = "data.cs";
    string filePath;

    void Awake()
    {
        playerProgressionModel.CurrentScoreChanged += Save;
    }

    void OnDestroy()
    {
        playerProgressionModel.CurrentScoreChanged -= Save;
    }

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) { CreateInitialSave(filePath); }
        stateData = ReadSaveFile(filePath);
    }

    void CreateInitialSave(string path)
    {
        stateData = new StateData()
        {
            Grid = CreateEmptyLinearGrid(gridModel.Width, gridModel.Height),
            NextPieces = CreateNextPieces(),
            CurrentScore = 0,
            TurnNumber = 0,
            LevelNumber = 1,
            RefreshesCount = 0,
            AddsCount = 0,
            ClearsCount = 0,
            BoostersGiven = 0,
            NextBoosterTurnNumber = 4
        };
        WriteSaveFile(stateData, path);
    }

    StateData ReadSaveFile(string path)
    {
        string data = File.ReadAllText(path);
        StateData saveData = JsonUtility.FromJson<StateData>(data);
        return saveData;
    }

    void WriteSaveFile(StateData saveData, string path)
    {
        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, jsonData);
    }

    int[] CreateEmptyLinearGrid(int width, int height)
    {
        int[] grid = new int[width * height];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = 0;
        }
        return grid;
    }

    int[] CreateNextPieces()
    {
        int piecesCount = piecesModel.Pieces.Length;
        return new int[] { Random.Range(1, piecesCount), Random.Range(1, piecesCount), Random.Range(1, piecesCount) };
    }

    void Save(int score)
    {
        stateData = new StateData()
        {
            Grid = CellsToIntegers(gridModel.Grid),
            NextPieces = CreateNextPieces(),
            CurrentScore = playerProgressionModel.CurrentScore,
            TurnNumber = playerProgressionModel.TurnNumber,
            LevelNumber = playerProgressionModel.LevelNumber,
            RefreshesCount = boostersModel.RefreshesCount,
            AddsCount = boostersModel.AddsCount,
            ClearsCount = boostersModel.ClearsCount,
            BoostersGiven = boostersModel.BoostersGiven,
            NextBoosterTurnNumber = boostersModel.NextBoosterTurnNumber
        };
        WriteSaveFile(stateData, filePath);
    }

    //Produces array from field left to right top to bottom
    int[] CellsToIntegers(Cell[,] cells)
    {
        int[] integers = new int[cells.GetLength(0) * cells.GetLength(1)];
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                integers[i + j * cells.GetLength(0)] = cells[i, cells.GetLength(1) - j - 1].Level;
            }
        }
        return integers;
    }

    int[] CellsToIntegers(Piece[] pieces)
    {
        int[] integers = new int[pieces.Length];
        for (int i = 0; i < integers.Length; i++)
        {
            integers[i] = pieces[i].Identifier;
        }
        return integers;
    }
}

[Serializable]
public struct StateData
{
    //Grid model
    public int[] Grid;
    //Pieces model
    public int[] NextPieces;
    //Player progression model
    public int CurrentScore;
    public int TurnNumber;
    public int LevelNumber;
    //Booster model
    public int RefreshesCount;
    public int AddsCount;
    public int ClearsCount;
    public int BoostersGiven;
    public int NextBoosterTurnNumber;
}