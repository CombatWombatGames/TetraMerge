using UnityEngine;
using UnityEngine.UI;

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
        if (count == 0)
        {
            refreshButton.interactable = false;
        }
        else
        {
            refreshButton.interactable = true;
        }
    }

    void OnAddsCountChanged(int count)
    {
        addsCount.text = count.ToString();
        if (count == 0)
        {
            //TODO Disable controller
        }
        else
        {

        }
    }

    void OnClearsCountChanged(int count)
    {
        clearsCount.text = count.ToString();
        if (count == 0)
        {
            //TODO Disable controller
        }
        else
        {

        }
    }
}
