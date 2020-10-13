using System;
using UnityEngine;

//Handles amount of boosters, gives boosters at appointed turns
public class BoostersModel : MonoBehaviour
{
    public event Action<int, BoosterType> BoosterCountChanged;

    public int RefreshesCount
    {
        get { return refreshesCount; }
        set
        {
            refreshesCount = value;
            BoosterCountChanged(value, BoosterType.Refresh);
        }
    }
    public int AddsCount
    {
        get { return addsCount; }
        set
        {
            addsCount = value;
            BoosterCountChanged(value, BoosterType.Add);
        }
    }
    public int ClearsCount
    {
        get { return clearsCount; }
        set
        {
            clearsCount = value;
            BoosterCountChanged(value, BoosterType.Clear);
        }
    }
    public int BoostersGiven { get; set; }
    public int NextBoosterTurnNumber
    {
        get
        {
            //Gives boosters on turn 5, 11, 18, 26, 35... Gap increases every time
            //BoostersGiven + 1 because it starts from zero
            return (BoostersGiven + 1) * (BoostersGiven + 10) / 2;
        }
    }

    GridModel gridModel;
    PiecesModel piecesModel;
    PlayerProgressionModel playerProgressionModel;

    int refreshesCount = 0;
    int addsCount = 0;
    int clearsCount = 0;

    void Awake()
    {
        gridModel = GetComponent<GridModel>();
        piecesModel = GetComponent<PiecesModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        playerProgressionModel.TurnChanged += OnTurnChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
    }

    public void Initialize(int refreshesCount, int addsCount, int clearsCount, int boostersGiven)
    {
        RefreshesCount = refreshesCount;
        AddsCount = addsCount;
        ClearsCount = clearsCount;
        BoostersGiven = boostersGiven;
    }

    void OnTurnChanged(int turnNumber)
    {
        if (turnNumber == NextBoosterTurnNumber)
        {
            RefreshesCount++;
            AddsCount++;
            ClearsCount++;
            BoostersGiven++;
        }
    }

    public void GenerateNewPieces()
    {
        if (RefreshesCount > 0)
        {
            piecesModel.GenerateNextPieces();
            RefreshesCount--;
            playerProgressionModel.TurnNumber++;
        }
    }

    public void ClearCell(Vector2Int position)
    {
        if (ClearsCount > 0)
        {
            gridModel.ChangeGrid(new Vector2Int[] { position }, 0);
            ClearsCount--;
            playerProgressionModel.TurnNumber++;
        }
    }

    public void AddCell(Vector2Int position)
    {
        if (AddsCount > 0)
        {
            gridModel.ChangeGrid(new Vector2Int[] { position }, playerProgressionModel.LevelNumber);
            AddsCount--;
            playerProgressionModel.TurnNumber++;
        }
    }
}

public enum BoosterType { Refresh, Add, Clear }