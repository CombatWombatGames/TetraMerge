﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Translates selecting and merging to model
public class GridController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    [SerializeField] Image selectionBox = default;
    [SerializeField] LineRenderer topLine = default;
    [SerializeField] LineRenderer middleLine = default;
    [SerializeField] LineRenderer bottomLine = default;

    //Shared variables for communication of methods from interfaces
    Vector3 beginDragWorldPosition;
    Vector2Int beginDragGridPosition;
    Vector2Int endDragGridPosition;
    Vector2Int[] validSelectedArea;
    Vector2Int[] areaToUpgrade;

    float scale;

    void Start()
    {
        scale = 1 / FindObjectOfType<Canvas>().transform.localScale.x;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragWorldPosition = eventData.pointerCurrentRaycast.worldPosition;
        beginDragGridPosition = gridView.WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        selectionBox.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Draw selection box
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            Vector3 cursorPosition = eventData.pointerCurrentRaycast.worldPosition;
            selectionBox.rectTransform.position = new Vector3((cursorPosition.x + beginDragWorldPosition.x) / 2, (cursorPosition.y + beginDragWorldPosition.y) / 2);
            selectionBox.rectTransform.sizeDelta = new Vector2(Mathf.Abs(cursorPosition.x - beginDragWorldPosition.x) * scale, Mathf.Abs(cursorPosition.y - beginDragWorldPosition.y) * scale);
        }
        //Draw selection shadow
        Vector2Int currentDragGridPosition = gridView.WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition);
        if (AreaIsSquare(beginDragGridPosition, currentDragGridPosition) && (beginDragGridPosition != currentDragGridPosition))
        {
            Vector2Int[] selectedArea = CalculateSelectedArea(beginDragGridPosition, currentDragGridPosition);
            if (AreaIsValid(selectedArea))
            {
                validSelectedArea = selectedArea; //OnEndDrag validSelectedArea will be merged...
                endDragGridPosition = currentDragGridPosition; //...using endDragGridPosition
                areaToUpgrade = CalculateAreaToUpgrade(beginDragGridPosition, currentDragGridPosition);
                gridView.DrawShadow(selectedArea, areaToUpgrade, true);
            }
            //Trying to disable selection drop if area is not valid
            //else
            //{
            //    validSelectedArea = null;
            //    gridView.DeleteShadow();
            //}
        }
        else if (beginDragGridPosition == currentDragGridPosition) //Remove condition to drop all non-valid selections
        {
            validSelectedArea = null;
            areaToUpgrade = null;
            gridView.DeleteShadow();
        }
    }

    bool AreaIsSquare(Vector2Int beginPosition, Vector2Int endPosition)
    {
        return Mathf.Abs(endPosition.x - beginPosition.x) == Mathf.Abs(endPosition.y - beginPosition.y);
    }

    Vector2Int[] CalculateSelectedArea(Vector2Int beginPosition, Vector2Int endPosition)
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

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBox.gameObject.SetActive(false);
        AnimationSystem.HideBorder(topLine, middleLine, bottomLine, beginDragWorldPosition, eventData.pointerCurrentRaycast.worldPosition, validSelectedArea != null);
        gridView.DeleteShadow();
        if (validSelectedArea != null)
        {
            int newLevel = gridModel.Grid[beginDragGridPosition.x, endDragGridPosition.y].Level + 1;
            Merge(validSelectedArea, areaToUpgrade, newLevel);
            validSelectedArea = null;
            areaToUpgrade = null;
        }
    }

    Vector2Int[] CalculateAreaToUpgrade(Vector2Int beginPosition, Vector2Int endPosition)
    {
        List<Vector2Int> upgradedArea = new List<Vector2Int>();
        //TODO LOW Find better solution
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
        return upgradedArea.ToArray();
    }

    void Merge(Vector2Int[] clearedArea, Vector2Int[] upgradedArea, int upgradeLevel)
    {
        gridModel.ChangeGrid(clearedArea, 0, GridChanger.Merge);
        if (clearedArea.Length == gridModel.Height * gridModel.Width)
        {
            gridModel.StageComplete();
            FindObjectOfType<MessageSystem>().ShowMessage(MessageId.StageChanged, 10f);
        }
        else
        {
            gridModel.ChangeGrid(upgradedArea, upgradeLevel, GridChanger.Merge);
        }
        playerProgressionModel.TurnNumber++;
        AudioSystem.Player.PlayMergeSfx(clearedArea.Length);
        AnimationSystem.ShakeField(gridView.Field, clearedArea.Length, gridView.DustParticles, gridView.ShardsParticles, gridView.LeafParticles, gridView.LeafParticlesBurst);
    }
}