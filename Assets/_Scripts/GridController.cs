using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] Image selectionBox = default;

    Vector3 beginDragWorldPosition;
    Vector2Int beginDragGridPosition;
    Vector2Int endDragPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragWorldPosition = eventData.pointerCurrentRaycast.worldPosition;
        beginDragGridPosition = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        Debug.Log(beginDragGridPosition);
        //TODO Move to greed veiw?
        selectionBox.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        selectionBox.rectTransform.position = new Vector3((eventData.pointerCurrentRaycast.worldPosition.x + beginDragWorldPosition.x) / 2, (eventData.pointerCurrentRaycast.worldPosition.y + beginDragWorldPosition.y) / 2);
        selectionBox.rectTransform.sizeDelta = new Vector2(Mathf.Abs(eventData.pointerCurrentRaycast.worldPosition.x - beginDragWorldPosition.x) * 100, Mathf.Abs(eventData.pointerCurrentRaycast.worldPosition.y - beginDragWorldPosition.y) * 100);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBox.gameObject.SetActive(false);
        endDragPosition = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        Debug.Log(endDragPosition);
    }

    Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    {
        float XGrid = worldCoordinate.x + (float)(gridModel.Width - 1) / 2;
        float YGrid = worldCoordinate.y + (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }
}
