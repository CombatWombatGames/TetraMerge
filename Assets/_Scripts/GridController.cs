using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Translates selecting and merging to model
public class GridController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] Image selectionBox = default;

    //Shared variables for communication of methods from interfaces
    Vector3 beginDragWorldPosition;
    Vector2Int beginDragGridPosition;
    Vector2Int endDragGridPosition;
    Vector2Int[] validSelectedArea;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragWorldPosition = eventData.pointerCurrentRaycast.worldPosition;
        beginDragGridPosition = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        //TODO Move to view?
        selectionBox.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Draw selection box
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            selectionBox.rectTransform.position = new Vector3((eventData.pointerCurrentRaycast.worldPosition.x + beginDragWorldPosition.x) / 2, (eventData.pointerCurrentRaycast.worldPosition.y + beginDragWorldPosition.y) / 2);
            selectionBox.rectTransform.sizeDelta = new Vector2(Mathf.Abs(eventData.pointerCurrentRaycast.worldPosition.x - beginDragWorldPosition.x) * 100, Mathf.Abs(eventData.pointerCurrentRaycast.worldPosition.y - beginDragWorldPosition.y) * 100);
        }
        //Draw selection shadow
        Vector2Int currentDragGridPosition = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        if (AreaIsSquare(beginDragGridPosition, currentDragGridPosition) && (beginDragGridPosition != currentDragGridPosition))
        {
            Vector2Int[] selectedArea = CalculateArea(beginDragGridPosition, currentDragGridPosition);
            if (AreaIsValid(selectedArea))
            {
                validSelectedArea = selectedArea; //OnEndDrag validSelectedArea will be merged...
                endDragGridPosition = currentDragGridPosition; //...using endDragGridPosition
                gridView.DrawShadow(selectedArea);
            }
            else
            {
                validSelectedArea = null;
                gridView.DeleteShadow();
            }
        }
        else if (beginDragGridPosition == currentDragGridPosition)
        {
            validSelectedArea = null;
            gridView.DeleteShadow();
        }
    }

    bool AreaIsSquare(Vector2Int beginPosition, Vector2Int endPosition)
    {
        return Mathf.Abs(endPosition.x - beginPosition.x) == Mathf.Abs(endPosition.y - beginPosition.y);
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

    bool AreaIsValid(Vector2Int[] area)
    {
        for (int i = 0; i < area.Length; i++)
        {
            if (!(0 <= area[i].x && area[i].x < gridModel.Width && 0 <= area[i].y && area[i].y < gridModel.Height))
            {
                //Selection is out of grid
                return false;
            }
            if (gridModel.Grid[area[i].x, area[i].y].Level == 0 || gridModel.Grid[area[i].x, area[i].y].Level != gridModel.Grid[beginDragGridPosition.x, beginDragGridPosition.y].Level)
            {
                //Cells are different
                return false;
            }
        }
        return true;
    }

    Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    {
        float XGrid = worldCoordinate.x / gridView.Scale + (float)(gridModel.Width - 1) / 2;
        float YGrid = worldCoordinate.y / gridView.Scale + (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBox.gameObject.SetActive(false);
        gridView.DeleteShadow();
        if (validSelectedArea != null)
        {
            Merge(validSelectedArea, beginDragGridPosition, endDragGridPosition);
            validSelectedArea = null;
        }
    }

    void Merge(Vector2Int[] area, Vector2Int beginPosition, Vector2Int endPosition)
    {
        List<Vector2Int> upgradedArea = new List<Vector2Int>();
        //TODO Find better solution
        if (endPosition.x - beginPosition.x > 0)
        {
            if (endPosition.y - beginPosition.y > 0)
            {
                for (int i = (endPosition.x + beginPosition.x) / 2 + 1; i <= endPosition.x; i++)
                {
                    for (int j = (endPosition.y + beginPosition.y) / 2 + 1; j <= endPosition.y; j++)
                    {
                        upgradedArea.Add(new Vector2Int(i, j));
                    }
                }
            }
            else
            {
                for (int i = (endPosition.x + beginPosition.x) / 2 + 1; i <= endPosition.x; i++)
                {
                    for (int j = (endPosition.y + beginPosition.y - 1) / 2; j >= endPosition.y; j--)
                    {
                        upgradedArea.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        else
        {
            if (endPosition.y - beginPosition.y > 0)
            {
                for (int i = (endPosition.x + beginPosition.x - 1) / 2; i >= endPosition.x; i--)
                {
                    for (int j = (endPosition.y + beginPosition.y) / 2 + 1; j <= endPosition.y; j++)
                    {
                        upgradedArea.Add(new Vector2Int(i, j));
                    }
                }
            }
            else
            {
                for (int i = (endPosition.x + beginPosition.x - 1) / 2; i >= endPosition.x; i--)
                {
                    for (int j = (endPosition.y + beginPosition.y - 1) / 2; j >= endPosition.y; j--)
                    {
                        upgradedArea.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        int newLevel = gridModel.Grid[beginPosition.x, beginPosition.y].Level + 1;
        gridModel.ChangeGrid(area, 0);
        gridModel.ChangeGrid(upgradedArea.ToArray(), newLevel);
    }
}