using UnityEngine;

public class BoostersModel : MonoBehaviour
{
    //TODO Disable controller if booster is not available
    public int RefreshesCount { get; set; }
    public int AddsCount { get; set; }
    public int ClearsCount { get; set; }

    void Start()
    {
        RefreshesCount = 1;
        AddsCount = 1;
        ClearsCount = 1;
    }
}
