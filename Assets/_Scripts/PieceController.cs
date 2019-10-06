using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //TODO Пул три штуки
    [SerializeField] GridModel gridModel = default;

    public void OnDrag(PointerEventData eventData)
    {
        //В редакторе перестает выполняться, когда курсор мыши за пределами объекта, но не заканчивает Drag! 
        //На телефоне работает как надо
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
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
    Vector2Int[] FindNearestArea(Vector3 worldPosition, PieceModel piece)
    {
        Array.Clear(nearestArea, 0, 9);
        for (int i = 0; i < piece.piece.Length; i++)
        {
            nearestArea[i] = PieceToGridCoordinate(worldPosition, piece.piece[i]);
        }
        return nearestArea;
    }

    Vector2Int PieceToGridCoordinate(Vector2 worldCoordinate, Cell cell)
    {
        float XGrid = worldCoordinate.x - (float)(gridModel.Width - 1) / 2;
        float YGrid = worldCoordinate.y - (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid) + cell.gridCoordinate.x, Mathf.RoundToInt(YGrid) + cell.gridCoordinate.y);
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
