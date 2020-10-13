using UnityEngine;
using UnityEngine.EventSystems;

//Translates movement, rotation and dropping of the piece to models
public class PieceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] PiecesView piecesView = default;
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    [SerializeField] int index = default;

    Vector3 shift = Vector3.up * 2;

    void Awake()
    {
        piecesModel.PiecesGenerated += ActivatePiece;
    }

    void OnDestroy()
    {
        piecesModel.PiecesGenerated -= ActivatePiece;
    }

    void ActivatePiece()
    {
        if (piecesModel.NextPieces[index].Identifier != -1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition + shift;
            Vector2Int[] nearestArea = FindNearestArea(eventData.pointerCurrentRaycast.worldPosition + shift, piecesModel.NextPieces[index]);
            if (AreaIsAvailable(nearestArea))
            {
                gridView.DrawShadow(nearestArea);
            }
            else
            {
                gridView.DeleteShadow();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        piecesView.ScalePiece(index, gridView.Scale * 1.42f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2Int[] nearestArea = FindNearestArea(eventData.pointerCurrentRaycast.worldPosition + shift, piecesModel.NextPieces[index]);
        if (AreaIsAvailable(nearestArea) && eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            //TODO LOW Ask for level another way. MB make entire piece level?
            gridModel.ChangeGrid(nearestArea, piecesModel.NextPieces[index].Cells[0].Level);
            gameObject.SetActive(false);
            piecesModel.RemovePiece(index);
            playerProgressionModel.TurnNumber++;
        }
        else
        {
            piecesView.ReturnPiece(index);
        }
        gridView.DeleteShadow();
        piecesView.ScalePiece(index, 1.0f / (gridView.Scale * 1.42f));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            piecesModel.RotatePiece(index);
        }
    }

    bool AreaIsAvailable(Vector2Int[] area)
    {
        for (int i = 0; i < area.Length; i++)
        {
            if (!(0 <= area[i].x && area[i].x < gridModel.Width && 0 <= area[i].y && area[i].y < gridModel.Height))
            {
                //Piece is out of grid
                return false;
            }
            if (gridModel.Grid[area[i].x, area[i].y].Level != 0)
            {
                //Cells are taken
                return false;
            }
        }
        return true;
    }

    Vector2Int[] FindNearestArea(Vector3 worldPosition, Piece piece)
    {
        Vector2Int[] rawNearestArea = new Vector2Int[piece.Cells.Length];
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            rawNearestArea[i] = PieceToGridCoordinate(piece.Cells[i], worldPosition);
        }
        return rawNearestArea;
    }

    Vector2Int PieceToGridCoordinate(Cell cell, Vector2 centerCoordinate)
    {
        var _centerCoordinate = gridView.WorldToGridCoordinate(centerCoordinate);
        float XGrid = _centerCoordinate.x + cell.GridCoordinate.x;
        float YGrid = _centerCoordinate.y + cell.GridCoordinate.y;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }
}
