using UnityEngine;
using UnityEngine.EventSystems;

public class PieceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] PiecesCollection piecesCollection = default;
    [SerializeField] int number = default;

    public void OnDrag(PointerEventData eventData)
    {
        //В редакторе перестает выполняться, когда курсор мыши за пределами объекта, но не заканчивает Drag! 
        //На телефоне работает как надо
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
            //Если ближайшее место свободно
            //Отрисовать там тень
            if (NearestAreaIsFree(eventData.pointerCurrentRaycast.worldPosition, piecesCollection.NextPieces[number]))
            {
                Debug.Log("Free");
            }
            else
            {
                Debug.Log("Taken");
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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

    bool NearestAreaIsFree(Vector2 centerPosition, Piece piece)
    {
        Vector2Int[] nearestArea = FindNearestArea(centerPosition, piece);
        for (int i = 0; i < nearestArea.Length; i++)
        {
            if (!(0 <= nearestArea[i].x && nearestArea[i].x < gridModel.Width && 0 <= nearestArea[i].y && nearestArea[i].y < gridModel.Height))
            {
                return false;
            }
            //Проверить уровень клеток поля
            //if (true)
            //{
            //    return false;
            //}
        }
        return true;
    }


    Vector2Int[] FindNearestArea(Vector3 worldPosition, Piece piece)
    {
        Vector2Int[] nearestArea = new Vector2Int[piece.Cells.Length];
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            nearestArea[i] = PieceToGridCoordinate(piece.Cells[i], worldPosition);
        }
        //for (int i = 0; i < nearestArea.Length; i++)
        //{
        //    Debug.Log($"{i}: {nearestArea[i].x}, {nearestArea[i].y}");
        //}
        return nearestArea;
    }

    Vector2Int PieceToGridCoordinate(Cell cell, Vector2 centerCoordinate)
    {
        float XGrid = centerCoordinate.x + cell.GridCoordinate.x + (float)(gridModel.Width - 1) / 2;
        float YGrid = centerCoordinate.y + cell.GridCoordinate.y + (float)(gridModel.Width - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }

    //Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    //{
    //    float XGrid = worldCoordinate.x - (float)(gridModel.Width - 1) / 2;
    //    float YGrid = worldCoordinate.y - (float)(gridModel.Height - 1) / 2;
    //    return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    //}

    //Vector2 GridToWorldCoordinate(Vector2Int gridCoordinate)
    //{
    //    float XWorld = gridCoordinate.x + (float)(gridModel.Width - 1) / 2;
    //    float YWorld = gridCoordinate.y + (float)(gridModel.Height - 1) / 2;
    //    return new Vector2(XWorld, YWorld);
    //}
}
