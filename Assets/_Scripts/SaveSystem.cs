using DG.Tweening;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//Takes care of saving, loading and undo
[DefaultExecutionOrder(-1000)]
public class SaveSystem : MonoBehaviour
{
    StateData stateData;

    [SerializeField] Button undoButton = default;

    GridModel gridModel;
    PlayerProgressionModel playerProgressionModel;
    BoostersModel boostersModel;
    PiecesModel piecesModel;
    string saveFileName = "data.json";
    string preveousSaveFileName = "olddata.json";
    string saveFilePath;
    string preveousSaveFilePath;
    bool initialized;

    void Awake()
    {
        gridModel = GetComponent<GridModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        boostersModel = GetComponent<BoostersModel>();
        piecesModel = GetComponent<PiecesModel>();
        PrepareStateData();
        AnalyticsSystem.Initialize(playerProgressionModel);
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
    }

    void Start()
    {
        //HACK subscribing in Start so saving would be called last
        playerProgressionModel.TurnChanged += OnTurnChanged;
        Load();
    }

    void PrepareStateData()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        preveousSaveFilePath = Path.Combine(Application.persistentDataPath, preveousSaveFileName);
        if (!File.Exists(saveFilePath)) { CreateInitialSave(saveFilePath); }
        stateData = ReadSaveFile(saveFilePath);
    }

    void Load()
    {
        gridModel.Initialize(stateData.Grid);
        piecesModel.Initialize(stateData.NextPieces, stateData.LevelNumber);
        boostersModel.Initialize(stateData.RefreshesCount, stateData.AddsCount, stateData.ClearsCount, stateData.BoostersGiven, stateData.UltimateUsed, stateData.BoostersOpen);
        playerProgressionModel.Initialize(stateData.CurrentScore, stateData.LevelNumber, stateData.TurnNumber, stateData.BestScore, stateData.BestLevel, stateData.BestRune, stateData.Stage, stateData.TotalMerged, stateData.TriesCount);
        undoButton.interactable = File.Exists(preveousSaveFilePath);
    }                            
                                 
    void CreateInitialSave(string path)
    {
        stateData = new StateData()
        {
            Grid = CreateEmptyLinearGrid(gridModel.Width, gridModel.Height),
            NextPieces = CreateNextPieces(),
            CurrentScore = 0,
            BestScore = playerProgressionModel.BestScore,
            BestLevel = playerProgressionModel.BestLevel,
            BestRune = playerProgressionModel.BestRune,
            Stage = 0,
            TotalMerged = playerProgressionModel.TotalMerged,
            TurnNumber = 0,
            LevelNumber = 1,
            TriesCount = playerProgressionModel.TriesCount,
            RefreshesCount = 0,
            AddsCount = 0,
            ClearsCount = 0,
            BoostersGiven = 0,
            UltimateUsed = true,
            BoostersOpen = false,
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
        int piecesCount = GetComponent<Resources>().PiecesList.Length;
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
        WriteSaveFile(stateData, preveousSaveFilePath);
        stateData = new StateData()
        {
            Grid = CellsToIntegers(gridModel.Grid),
            NextPieces = PiecesToIntegers(piecesModel.NextPieces),
            CurrentScore = playerProgressionModel.CurrentScore,
            TurnNumber = playerProgressionModel.TurnNumber,
            LevelNumber = playerProgressionModel.LevelNumber,
            BestScore = playerProgressionModel.BestScore,
            BestLevel = playerProgressionModel.BestLevel,
            BestRune = playerProgressionModel.BestRune,
            Stage = playerProgressionModel.Stage,
            TotalMerged = playerProgressionModel.TotalMerged,
            TriesCount = playerProgressionModel.TriesCount,
            RefreshesCount = boostersModel.RefreshesCount,
            AddsCount = boostersModel.AddsCount,
            ClearsCount = boostersModel.ClearsCount,
            BoostersGiven = boostersModel.BoostersGiven,
            UltimateUsed = boostersModel.UltimateUsed,
            BoostersOpen = boostersModel.BoostersOpen,
        };
        WriteSaveFile(stateData, saveFilePath);
        undoButton.interactable = true;
    }

    public void Undo()
    {
        File.Delete(saveFilePath);
        File.Move(preveousSaveFilePath, saveFilePath);
        DOTween.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartFromScratch()
    {
        playerProgressionModel.TriesCount++;
        Save();
        CreateInitialSave(saveFilePath);
        DOTween.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AnalyticsSystem.Restart(playerProgressionModel.LevelNumber);
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
    public int BestScore;
    public int BestLevel;
    public int BestRune;
    public int Stage;
    public int TotalMerged;
    public int TriesCount;
    //Booster model
    public int RefreshesCount;
    public int AddsCount;
    public int ClearsCount;
    public int BoostersGiven;
    public bool UltimateUsed;
    public bool BoostersOpen;
}