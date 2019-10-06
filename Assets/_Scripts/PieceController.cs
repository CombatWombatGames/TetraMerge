﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //TODO Пул три штуки
    [SerializeField] GridModel gridModel = default;
    [SerializeField] PiecesCollection piecesCollection = default;

    //PieceModel pieceModel = new PieceModel();
    //private void Start()
    //{
    //    pieceModel.piece = new Cell[] { new Vector2Int(0, -1);
    //};

    public void OnDrag(PointerEventData eventData)
    {
        //В редакторе перестает выполняться, когда курсор мыши за пределами объекта, но не заканчивает Drag! 
        //На телефоне работает как надо
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
            FindNearestArea(eventData.pointerCurrentRaycast.worldPosition, piecesCollection.Pieces[0]);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Если ближайшее место свободно
        //Отрисовать тень
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Если рисовалась тень
        //Плавно поставить на место тени
        //Обновить модель сетки
        //Иначе
        //Вернуть тайл на место
    }

    void Rotate()
    {

    }

    bool NearestAreaIsFree(Cell[] cells)
    {

        return true;
    }

    Vector2Int[] nearestArea = new Vector2Int[9];
    Vector2Int[] FindNearestArea(Vector3 worldPosition, Piece piece)
    {
        Array.Clear(nearestArea, 0, 9);
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            nearestArea[i] = PieceToGridCoordinate(worldPosition, piece.Cells[i]);
        }
        //Debug.Log($"{nearestArea[0].x},   {nearestArea[0].y}");
        return nearestArea;
    }

    Vector2Int PieceToGridCoordinate(Vector2 centerCoordinate, Cell cell)
    {
        float XGrid = centerCoordinate.x + cell.GridCoordinate.x + (float)(gridModel.Width - 1) / 2;
        float YGrid = centerCoordinate.y + cell.GridCoordinate.y + (float)(gridModel.Width - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }

    Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    {
        float XGrid = worldCoordinate.x - (float)(gridModel.Width - 1) / 2;
        float YGrid = worldCoordinate.y - (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }

    Vector2 GridToWorldCoordinate(Vector2Int gridCoordinate)
    {
        float XWorld = gridCoordinate.x + (float)(gridModel.Width - 1) / 2;
        float YWorld = gridCoordinate.y + (float)(gridModel.Height - 1) / 2;
        return new Vector2(XWorld, YWorld);
    }
}