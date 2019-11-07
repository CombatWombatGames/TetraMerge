using UnityEngine;
using UnityEngine.UI;

//Shows amount of boosters to player
public class BoostersView : MonoBehaviour
{
    [SerializeField] BoostersModel boostersModel = default;
    [SerializeField] Button refreshButton = default;
    [SerializeField] Text refreshesCount = default;
    [SerializeField] Text addsCount = default;
    [SerializeField] Text clearsCount = default;

    void Awake()
    {
        boostersModel.RefreshesCountChanged += OnRefreshesCountChanged;
        boostersModel.AddsCountChanged += OnAddsCountChanged;
        boostersModel.ClearsCountChanged += OnClearsCountChanged;
    }

    void OnDestroy()
    {
        boostersModel.RefreshesCountChanged -= OnRefreshesCountChanged;
        boostersModel.AddsCountChanged -= OnAddsCountChanged;
        boostersModel.ClearsCountChanged -= OnClearsCountChanged;
    }

    void OnRefreshesCountChanged(int count)
    {
        refreshesCount.text = count.ToString();
        if (count != 0)
        {
            refreshButton.interactable = true;
        }
        else
        {
            refreshButton.interactable = false;
        }
    }

    void OnAddsCountChanged(int count)
    {
        addsCount.text = count.ToString();
    }

    void OnClearsCountChanged(int count)
    {
        clearsCount.text = count.ToString();
    }
}
