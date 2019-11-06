using UnityEngine;
using UnityEngine.EventSystems;

//Provides methods for booster buttons
public class BoosterController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] BoostersModel boostersModel = default;
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    //TODO Use inheritance instead
    [SerializeField] BoosterType boosterType = default;

    int turnsToGiveBoosters = 10;
    Vector3 shift = Vector3.up * 4;

    void Awake()
    {
        playerProgressionModel.TurnChanged += OnTurnChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
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
        //TODO Scale
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

    enum BoosterType { Refresh, Add, Clear }
}
