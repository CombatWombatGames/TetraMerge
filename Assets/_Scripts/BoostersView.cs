﻿using UnityEngine;
using UnityEngine.UI;

//Shows amount of boosters to player
public class BoostersView : MonoBehaviour
{
    [SerializeField] Button ultimateButton = default;
    [SerializeField] Button refreshButton = default;
    [SerializeField] Text refreshesCount = default;
    [SerializeField] Text addsCount = default;
    [SerializeField] Text clearsCount = default;
    [SerializeField] Text addsRaycastTarget = default;
    [SerializeField] Text clearsRaycastTarget = default;
    [SerializeField] Image addsIcon = default;
    [SerializeField] Image clearsIcon = default;
    [SerializeField] Slider slider = default;

    BoostersModel boostersModel;
    PlayerProgressionModel playerProgressionModel;
    MessageSystem messageSystem;

    void Awake()
    {
        boostersModel = GetComponent<BoostersModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        messageSystem = GetComponent<MessageSystem>();
        playerProgressionModel.TurnChanged += OnTurnChanged;
        boostersModel.BoosterCountChanged += OnBoostersCountChanged;
        boostersModel.RandomBoostersAcquired += OnRandomBoostersAcquired;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
        boostersModel.BoosterCountChanged -= OnBoostersCountChanged;
        boostersModel.RandomBoostersAcquired -= OnRandomBoostersAcquired;
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
        refreshesCount.text = $"{count}/{boostersModel.BoostersLimit}";
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
        addsCount.text = $"{count}/{boostersModel.BoostersLimit}";
        if (count == 0)
        {
            addsRaycastTarget.raycastTarget = false;
            addsIcon.color = new Color(0.87f, 0.87f, 0.87f, 0.5f);
        }
        else
        {
            addsRaycastTarget.raycastTarget = true;
            addsIcon.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    void OnClearsCountChanged(int count)
    {
        clearsCount.text = $"{count}/{boostersModel.BoostersLimit}";
        if (count == 0)
        {
            clearsRaycastTarget.raycastTarget = false;
            clearsIcon.color = new Color(0.87f, 0.87f, 0.87f, 0.5f);
        }
        else
        {
            clearsRaycastTarget.raycastTarget = true;
            clearsIcon.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    void OnTurnChanged(int turnNumber)
    {
        var value = (float)(turnNumber - boostersModel.PreviousBoosterTurnNumber) / (boostersModel.NextBoosterTurnNumber - boostersModel.PreviousBoosterTurnNumber);
        AnimationSystem.ChangeProgress(slider, value);
        ultimateButton.gameObject.SetActive(!boostersModel.UltimateUsed);
    }

    void OnRandomBoostersAcquired(int count)
    {
        if (count > 0)
        {
            messageSystem.ShowMessage(MessageId.BoostersIncremented);
        }
    }
}
