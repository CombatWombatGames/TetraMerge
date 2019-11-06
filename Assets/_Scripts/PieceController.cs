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

    Vector3 shift = Vector3.up * 4;

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
        gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition + shift;
            Vector2Int[] nearestArea = FindNearestArea(eventData.pointerCurrentRaycast.worldPosition + shift, piecesModel.NextPieces[index]);
            if (AreaIsAvailable(nearestArea))
            {
                gridView.DrawPieceShadow(nearestArea);
            }
            else
            {
                gridView.DeletePieceShadow();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2Int[] nearestArea = FindNearestArea(eventData.pointerCurrentRaycast.worldPosition + shift, piecesModel.NextPieces[index]);
        if (AreaIsAvailable(nearestArea))
        {
            gridModel.ChangeGrid(nearestArea, piecesModel.NextPieces[index].Cells[0].Level);
            gameObject.SetActive(false);
            piecesModel.RemovePiece(index);
            playerProgressionModel.TurnNumber++;
        }
        else
        {
            piecesView.ReturnPiece(index);
        }
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
        float XGrid = cell.GridCoordinate.x + centerCoordinate.x / gridView.Scale + (float)(gridModel.Width - 1) / 2;
        float YGrid = cell.GridCoordinate.y + centerCoordinate.y / gridView.Scale + (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }

    //Vector2 GridToWorldCoordinate(Vector2Int gridCoordinate)
    //{
    //    float XWorld = gridCoordinate.x + (float)(gridModel.Width - 1) / 2;
    //    float YWorld = gridCoordinate.y + (float)(gridModel.Height - 1) / 2;
    //    return new Vector2(XWorld, YWorld);
    //}
}
