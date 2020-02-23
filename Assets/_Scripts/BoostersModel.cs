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
    public int BoostersGiven
    {
        get { return boostersGiven; }
        set
        {
            boostersGiven = value;
            UpdateNextBoosterTurnNumber();
        }
    }
    public int NextBoosterTurnNumber { get; set; }

    [SerializeField] PlayerProgressionModel playerProgressionModel = default;

    int refreshesCount = 0;
    int addsCount = 0;
    int clearsCount = 0;
    int boostersGiven = 0;

    void Awake()
    {
        playerProgressionModel.TurnChanged += OnTurnChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
    }

    public void Initialize(int refreshesCount, int addsCount, int clearsCount, int boostersGiven, int nextBoosterTurnNumber)
    {
        RefreshesCount = refreshesCount;
        AddsCount = addsCount;
        ClearsCount = clearsCount;
        BoostersGiven = boostersGiven;
        NextBoosterTurnNumber = nextBoosterTurnNumber;
    }

    void UpdateNextBoosterTurnNumber()
    {
        //Gives boosters on turn 4, 9, 15, 22, 30... Gap increases every time
        NextBoosterTurnNumber = (BoostersGiven + 1) * (BoostersGiven + 8) / 2;
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
}

public enum BoosterType { Refresh, Add, Clear }