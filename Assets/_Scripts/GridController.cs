using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] Image selectionBox = default;

    Vector3 beginDragWorldPosition;
    Vector2Int beginDragGridPosition;
    Vector2Int endDragPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragWorldPosition = eventData.pointerCurrentRaycast.worldPosition;
        beginDragGridPosition = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        //Debug.Log(beginDragGridPosition);
        //TODO Move to grid view?
        selectionBox.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Draw selection box
        selectionBox.rectTransform.position = new Vector3((eventData.pointerCurrentRaycast.worldPosition.x + beginDragWorldPosition.x) / 2, (eventData.pointerCurrentRaycast.worldPosition.y + beginDragWorldPosition.y) / 2);
        selectionBox.rectTransform.sizeDelta = new Vector2(Mathf.Abs(eventData.pointerCurrentRaycast.worldPosition.x - beginDragWorldPosition.x) * 100, Mathf.Abs(eventData.pointerCurrentRaycast.worldPosition.y - beginDragWorldPosition.y) * 100);
        //Draw selection shadow
        gridView.DrawSelectionShadow(CalculateArea(beginDragGridPosition, WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition)));
    }

    Vector2Int[] CalculateArea(Vector2Int beginPosition, Vector2Int endPosition)
    {
        int columnsCount = Mathf.Abs(endPosition.x - beginPosition.x) + 1;
        int rowsCount = Mathf.Abs(endPosition.y - beginPosition.y) + 1;
        Vector2Int[] area = new Vector2Int[columnsCount * rowsCount];
        int firstColumnNumber = (endPosition.x > beginPosition.x) ? beginPosition.x : endPosition.x;
        int firstRowNumber = (endPosition.y > beginPosition.y) ? beginPosition.y : endPosition.y;
        int index = 0;
        for (int i = firstColumnNumber; i < firstColumnNumber + columnsCount; i++)
        {
            for (int j = firstRowNumber; j < firstRowNumber + rowsCount; j++)
            {
                area[index] = new Vector2Int(i, j);
                index++;
            }
        }
        index = 0;
        return area;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBox.gameObject.SetActive(false);
        endDragPosition = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        //Debug.Log(endDragPosition);
    }

    Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    {
        float XGrid = worldCoordinate.x + (float)(gridModel.Width - 1) / 2;
        float YGrid = worldCoordinate.y + (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }
}
