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
    public int BoostersGiven { get; private set; }
    public bool UltimateUsed { get; set; }
    public bool BoostersOpen { get; set; }
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
        piecesModel.CollectionLevelUp += OnCollectionLevelUp;
        gridModel.CellsMerged += OnCellsMerged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
        piecesModel.CollectionLevelUp -= OnCollectionLevelUp;
        gridModel.CellsMerged -= OnCellsMerged;
    }

    public void Initialize(int refreshesCount, int addsCount, int clearsCount, int boostersGiven, bool ultimateUsed, bool boostersOpen)
    {
        RefreshesCount = refreshesCount;
        AddsCount = addsCount;
        ClearsCount = clearsCount;
        BoostersGiven = boostersGiven;
        UltimateUsed = ultimateUsed;
        BoostersOpen = boostersOpen;
    }

    void OnTurnChanged(int turnNumber)
    {
        if (turnNumber == NextBoosterTurnNumber)
        {
            for (int i = 0; i < 3; i++)
            {
                GiveRandomBooster();
            }
            BoostersGiven++;
        }

        if (playerProgressionModel.Stage == 0 && BoostersOpen && !AnyBoosterAvailable())
        {
            UltimateUsed = false;
        }
    }

    public void GenerateNewPieces()
    {
        if (RefreshesCount > 0)
        {
            piecesModel.GenerateNextPieces();
            RefreshesCount--;
            playerProgressionModel.TurnNumber++;
            AnalyticsSystem.BoosterUsed(BoosterType.Refresh);
        }
    }

    public void ClearCell(Vector2Int position)
    {
        if (ClearsCount > 0)
        {
            gridModel.ChangeGrid(new Vector2Int[] { position }, 0);
            ClearsCount--;
            playerProgressionModel.TurnNumber++;
            AnalyticsSystem.BoosterUsed(BoosterType.Clear);
        }
    }

    public void AddCell(Vector2Int position)
    {
        if (AddsCount > 0)
        {
            gridModel.ChangeGrid(new Vector2Int[] { position }, playerProgressionModel.LevelNumber);
            AddsCount--;
            playerProgressionModel.TurnNumber++;
            AnalyticsSystem.BoosterUsed(BoosterType.Add);
        }
    }

    public void ClearBasicRunes()
    {
        UltimateUsed = true;
        RefreshesCount = 0;
        AddsCount = 0;
        ClearsCount = 0;
        gridModel.RemoveMinimumLevelPieces();
        playerProgressionModel.TurnNumber++;
        AnalyticsSystem.BoosterUsed(BoosterType.Ultimate);
    }

    public bool AnyBoosterAvailable()
    {
        return AddsCount > 0 || ClearsCount > 0 || RefreshesCount > 0;
    }

    void GiveRandomBooster()
    {
        int index = UnityEngine.Random.Range(0, 3);
        switch (index)
        {
            case 0:
                RefreshesCount++;
                AnalyticsSystem.BoosterAcquired(BoosterType.Refresh);
                break;
            case 1:
                AddsCount++;
                AnalyticsSystem.BoosterAcquired(BoosterType.Add);
                break;
            case 2:
                ClearsCount++;
                AnalyticsSystem.BoosterAcquired(BoosterType.Clear);
                break;
            default:
                break;
        }
    }

    void OnCellsMerged(int area)
    {
        if (9 < area && area < 36)
        {
            GiveRandomBooster();
        }
    }

    void OnCollectionLevelUp()
    {
        //TODO MED Show message
        for (int i = 0; i < 2; i++)
        {
            GiveRandomBooster();
        }
    }
}

public enum BoosterType { Refresh, Add, Clear, Ultimate }