using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-1000)]
public class SaveSystem : MonoBehaviour
{
    StateData stateData;

    [SerializeField] GridModel gridModel = default;
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    [SerializeField] BoostersModel boostersModel = default;
    [SerializeField] PiecesModel piecesModel = default;

    string fileName = "data.json";
    string filePath;
    bool initialized;

    void Awake()
    {
        playerProgressionModel.TurnChanged += OnTurnChanged;
        PrepareStateData();
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
    }

    void Start()
    {
        Load();
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void PrepareStateData()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) { CreateInitialSave(filePath); }
        stateData = ReadSaveFile(filePath);
    }

    void Load()
    {
        gridModel.Initialize(stateData.Grid);
        piecesModel.Initialize(stateData.NextPieces, stateData.LevelNumber);
        playerProgressionModel.Initialize(stateData.CurrentScore, stateData.LevelNumber, stateData.TurnNumber);
        boostersModel.Initialize(stateData.RefreshesCount, stateData.AddsCount, stateData.ClearsCount, stateData.BoostersGiven, stateData.NextBoosterTurnNumber);
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
        int piecesCount = piecesModel.CountPiecesVariants();
        return new int[] { Random.Range(1, piecesCount), Random.Range(1, piecesCount), Random.Range(1, piecesCount) };
    }

    void OnTurnChanged(int turn)
    {
        //Do not save if turn sets up during loading
        if (initialized)
        {
            Save();
        }
        else
        {
            initialized = true;
        }
    }

    void Save()
    {
        stateData = new StateData()
        {
            Grid = CellsToIntegers(gridModel.Grid),
            NextPieces = PiecesToIntegers(piecesModel.NextPieces),
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

    public void StartFromScratch()
    {
        CreateInitialSave(filePath);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    int[] PiecesToIntegers(Piece[] pieces)
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