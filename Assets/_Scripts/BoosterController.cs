using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Provides methods for booster buttons
public class BoosterController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] BoostersModel boostersModel = default;
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    //TODO Use inheritance instead!
    [SerializeField] BoosterType boosterType = default;
    [SerializeField] Text raycastTarget = default;

    int turnsToGiveBoosters = 10;
    Vector3 shift = Vector3.up * 4;

    void Awake()
    {
        playerProgressionModel.TurnChanged += OnTurnChanged;
        boostersModel.AddsCountChanged += OnAddsCountChanged;
        boostersModel.ClearsCountChanged += OnClearsCountChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
        boostersModel.AddsCountChanged -= OnAddsCountChanged;
        boostersModel.ClearsCountChanged -= OnClearsCountChanged;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero && boosterType != BoosterType.Refresh)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition + shift;
            Vector2Int nearestCell = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition + shift);
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
        //TODO Move to view
        transform.localScale *= gridView.Scale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (boosterType != BoosterType.Refresh)
        {
            Vector2Int nearestCell = WorldToGridCoordinate(eventData.pointerCurrentRaycast.worldPosition + shift);
            if (CellIsAvailable(nearestCell) && eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
            {
                if (boosterType == BoosterType.Clear)
                {
                    ClearCell(nearestCell);
                }
                else if (boosterType == BoosterType.Add)
                {
                    AddCell(nearestCell);
                }
            }
            transform.localPosition = Vector3.zero;
            gridView.DeletePieceShadow();
            transform.localScale /= gridView.Scale;
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
            gridModel.ChangeGrid(new Vector2Int[] { position }, playerProgressionModel.LevelNumber);
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

    void OnTurnChanged(int turnNumber)
    {
        if (turnNumber % turnsToGiveBoosters == 0)
        {
            if (boosterType == BoosterType.Refresh)
            {
                boostersModel.RefreshesCount++;
            }
            else if (boosterType == BoosterType.Add)
            {
                boostersModel.AddsCount++;
            }
            else if (boosterType == BoosterType.Clear)
            {
                boostersModel.ClearsCount++;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (boosterType != BoosterType.Refresh)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition + shift;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (boosterType != BoosterType.Refresh)
        {
            transform.localPosition = Vector3.zero;
            gridView.DeletePieceShadow();
        }
    }

    void OnClearsCountChanged(int count)
    {
        if (boosterType == BoosterType.Clear)
        {
            if (count != 0)
            {
                raycastTarget.raycastTarget = true;
            }
            else
            {
                raycastTarget.raycastTarget = false;
            }
        }
    }

    void OnAddsCountChanged(int count)
    {
        if (boosterType == BoosterType.Add)
        {
            if (count != 0)
            {
                raycastTarget.raycastTarget = true;
            }
            else
            {
                raycastTarget.raycastTarget = false;
            }
        }
    }

    enum BoosterType { Refresh, Add, Clear }
}
