using System;
using UnityEngine;

//Handles amount of boosters
public class BoostersModel : MonoBehaviour
{
    public event Action<int> RefreshesCountChanged;
    public event Action<int> AddsCountChanged;
    public event Action<int> ClearsCountChanged;

    public int RefreshesCount
    {
        get { return refreshesCount; }
        set
        {
            refreshesCount = value;
            RefreshesCountChanged(value);
        }
    }
    public int AddsCount
    {
        get { return addsCount; }
        set
        {
            addsCount = value;
            AddsCountChanged(value);
        }
    }
    public int ClearsCount
    {
        get { return clearsCount; }
        set
        {
            clearsCount = value;
            ClearsCountChanged(value);
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
    public int NextBoosterTurnNumber { get; set; } = 4;

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

    void UpdateNextBoosterTurnNumber()
    {
        //Gives boosters on turn 4, 9, 15, 22, 30... Gap increments every time
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