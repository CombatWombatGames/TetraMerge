using UnityEngine;
using UnityEngine.UI;

//Shows amount of boosters to player
public class BoostersView : MonoBehaviour
{
    [SerializeField] Button refreshButton = default;
    [SerializeField] Text refreshesCount = default;
    [SerializeField] Text addsCount = default;
    [SerializeField] Text clearsCount = default;
    [SerializeField] Text addsRaycastTarget = default;
    [SerializeField] Text clearsRaycastTarget = default;
    [SerializeField] Slider slider = default;

    BoostersModel boostersModel;
    PlayerProgressionModel playerProgressionModel;

    void Awake()
    {
        boostersModel = GetComponent<BoostersModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        playerProgressionModel.TurnChanged += OnTurnChanged;
        boostersModel.BoosterCountChanged += OnBoostersCountChanged;
        playerProgressionModel.LevelNumberChanged += OnLevelNumberChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
        boostersModel.BoosterCountChanged -= OnBoostersCountChanged;
        playerProgressionModel.LevelNumberChanged -= OnLevelNumberChanged;
    }

    private void OnLevelNumberChanged(int number)
    {
    }

    void OnBoostersCountChanged(int count, BoosterType type)
    {
        switch (type)
        {
            case BoosterType.Refresh:
                OnRefreshesCountChanged(count);
                break;
            case BoosterType.Add:
                OnAddsCountChanged(count);
                break;
            case BoosterType.Clear:
                OnClearsCountChanged(count);
                break;
            default:
                break;
        }
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
            addsRaycastTarget.raycastTarget = false;
        }
        else
        {
            addsRaycastTarget.raycastTarget = true;
        }
    }

    void OnClearsCountChanged(int count)
    {
        clearsCount.text = count.ToString();
        if (count == 0)
        {
            clearsRaycastTarget.raycastTarget = false;
        }
        else
        {
            clearsRaycastTarget.raycastTarget = true;
        }
    }

    void OnTurnChanged(int turnNumber)
    {
        slider.value = (float)(turnNumber - boostersModel.PreviousBoosterTurnNumber) / (boostersModel.NextBoosterTurnNumber - boostersModel.PreviousBoosterTurnNumber);
    }
}
