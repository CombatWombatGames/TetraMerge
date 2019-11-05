﻿using UnityEngine;
using UnityEngine.EventSystems;

//Provides methods for booster buttons
public class BoosterController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] BoostersModel boostersModel = default;
    //TODO Use inheritance instead
    [SerializeField] BoosterType boosterType = default;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero && boosterType != BoosterType.Refresh)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
            Vector2Int nearestCell = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
            if (CellIsAvailable(nearestCell))
            {
                gridView.DrawPieceShadow(new Vector2Int[] { nearestCell });
            }
            else
            {
                gridView.DeletePieceShadow();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (boosterType != BoosterType.Refresh)
        {
            Vector2Int nearestCell = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
            if (CellIsAvailable(nearestCell))
            {
                if (boosterType == BoosterType.Clear)
                {
                    ClearCell(nearestCell);
                    transform.localPosition = Vector3.zero;
                }
                else if (boosterType == BoosterType.Add)
                {
                    AddCell(nearestCell);
                    transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }
    }

    //UGUI
    public void GenerateNewPieces()
    {
        if (boostersModel.RefreshesCount > 0)
        {
            piecesModel.GenerateNextPieces();
            boostersModel.RefreshesCount--;
        }
    }

    public void ClearCell(Vector2Int position)
    {
        if (boostersModel.ClearsCount > 0)
        {
            gridModel.ChangeGrid(new Vector2Int[] { position }, 0);
            boostersModel.ClearsCount--;
        }
    }

    public void AddCell(Vector2Int position)
    {
        if (boostersModel.AddsCount > 0)
        {
            gridModel.ChangeGrid(new Vector2Int[] { position }, gridModel.MinimumLevel);
            boostersModel.AddsCount--;
        }
    }

    Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    {
        float XGrid = worldCoordinate.x / gridView.Scale + (float)(gridModel.Width - 1) / 2;
        float YGrid = worldCoordinate.y / gridView.Scale + (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
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

    enum BoosterType { Refresh, Add, Clear }
}