﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Provides methods for booster buttons
//TODO LOW Rewrite
public class BoosterController : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] BoostersModel boostersModel = default;
    //TODO MED Use inheritance instead!
    [SerializeField] BoosterType boosterType = default;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero && boosterType != BoosterType.Refresh)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition + gridView.BoosterFingerShift;
            Vector2Int nearestCell = gridView.WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition + gridView.BoosterFingerShift);
            if (CellIsAvailable(nearestCell))
            {
                var areaToClear = new Vector2Int[] { nearestCell };
                Vector2Int[] areaToUpgrade = null;
                if (boosterType == BoosterType.Add)
                {
                    areaToUpgrade = areaToClear;
                }
                gridView.DrawShadow(areaToClear, areaToUpgrade, true);
            }
            else
            {
                gridView.DeleteShadow();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (boosterType != BoosterType.Refresh)
        {
            Vector2Int nearestCell = gridView.WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition + gridView.BoosterFingerShift);
            if (CellIsAvailable(nearestCell) && eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
            {
                if (boosterType == BoosterType.Clear)
                {
                    boostersModel.ClearCell(nearestCell);
                }
                else if (boosterType == BoosterType.Add)
                {
                    boostersModel.AddCell(nearestCell);
                }
                AudioSystem.Player.PlayBoosterSfx();
            }
            transform.localPosition = Vector3.zero;
            gridView.DeleteShadow();
        }
    }

    bool CellIsAvailable(Vector2Int position)
    {
        if (!(0 <= position.x && position.x < gridModel.Width && 0 <= position.y && position.y < gridModel.Height))
        {
            //Out of grid
            return false;
        }
        if (boosterType == BoosterType.Add && gridModel.Grid[position.x, position.y].Level == 0)
        {
            return true;
        }
        else if (boosterType == BoosterType.Clear && gridModel.Grid[position.x, position.y].Level != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (boosterType != BoosterType.Refresh)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition + gridView.BoosterFingerShift;
            if (boosterType != BoosterType.Refresh)
            {
                transform.localScale *= 0.75f;
                GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            AudioSystem.Player.PlayRaiseSfx();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (boosterType != BoosterType.Refresh)
        {
            transform.localScale /= 0.75f;
            GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
            transform.localPosition = Vector3.zero;
            gridView.DeleteShadow();
            AudioSystem.Player.PlayTurnSfx();
        }
    }
}