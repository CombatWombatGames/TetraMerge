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

    Cell[] FindNearestArea()
    {
        //перевести координату каждой клетки в сеточные
        //для каждой проверить, входит ли в границы поля
        //если все входят - вернуть массив координат
        return null;
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
