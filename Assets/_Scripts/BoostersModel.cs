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
            //Gives boosters on turn 10, 21, 33, 46, 60... Gap increases every time
            //BoostersGiven + 1 because it starts from zero
            return (BoostersGiven + 1) * (BoostersGiven + 20) / 2;
        }
    }
    public int PreviousBoosterTurnNumber
    {
        get
        {
            //Gives boosters on turn 10, 21, 33, 46, 60... Gap increases every time
            //BoostersGiven + 1 because it starts from zero
            return BoostersGiven * (BoostersGiven + 19) / 2;
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
        gridModel.GridChanged += OnGridChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
        gridModel.GridChanged -= OnGridChanged;
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
            for (int i = 0; i < 2; i++)
            {
                GiveRandomBooster();
            }
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

    void GiveRandomBooster()
    {
        int index = UnityEngine.Random.Range(0, 3);
        switch (index)
        {
            case 0:
                RefreshesCount++;
                break;
            case 1:
                AddsCount++;
                break;
            case 2:
                ClearsCount++;
                break;
            default:
                break;
        }
    }

    void OnGridChanged(Vector2Int[] area, int level)
    {
        if (level == 0)
        {
            if (area.Length > 25)
            {
                for (int i = 0; i < 3; i++)
                {
                    GiveRandomBooster();
                }
            }
            else if (area.Length > 16)
            {
                for (int i = 0; i < 2; i++)
                {
                    GiveRandomBooster();
                }
            }
            else if (area.Length > 9)
            {
                GiveRandomBooster();
            }
        }
    }
}

public enum BoosterType { Refresh, Add, Clear }