using System;
using UnityEngine;

//Holds amount of boosters
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

    int refreshesCount = 0;
    int addsCount = 0;
    int clearsCount = 0;
}