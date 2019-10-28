using UnityEngine;
using UnityEngine.EventSystems;

//Translates player actions to model
public class PieceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GridView gridView = default;
    [SerializeField] PiecesView piecesView = default;
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] int index = default;
    Vector2Int[] nearestArea;

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
        //В редакторе перестает выполняться, когда курсор мыши за пределами объекта, но не заканчивает Drag! 
        //На телефоне работает как надо
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
            if (NearestAreaIsAvailable(eventData.pointerCurrentRaycast.worldPosition, piecesModel.NextPieces[index]))
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
        if (NearestAreaIsAvailable(eventData.pointerCurrentRaycast.worldPosition, piecesModel.NextPieces[index]))
        {
            gridModel.ChangeGrid(nearestArea, piecesModel.NextPieces[index].Cells[0].Level);
            gameObject.SetActive(false);
            piecesModel.RemovePiece(index);
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

    bool NearestAreaIsAvailable(Vector2 centerPosition, Piece piece)
    {
        nearestArea = FindNearestArea(centerPosition, piece);
        for (int i = 0; i < nearestArea.Length; i++)
        {
            if (!(0 <= nearestArea[i].x && nearestArea[i].x < gridModel.Width && 0 <= nearestArea[i].y && nearestArea[i].y < gridModel.Height))
            {
                //Piece is out of grid
                return false;
            }
            if (gridModel.Grid[nearestArea[i].x, nearestArea[i].y].Level != 0)
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
        float XGrid = centerCoordinate.x + cell.GridCoordinate.x + (float)(gridModel.Width - 1) / 2;
        float YGrid = centerCoordinate.y + cell.GridCoordinate.y + (float)(gridModel.Width - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }

    //Vector2 GridToWorldCoordinate(Vector2Int gridCoordinate)
    //{
    //    float XWorld = gridCoordinate.x + (float)(gridModel.Width - 1) / 2;
    //    float YWorld = gridCoordinate.y + (float)(gridModel.Height - 1) / 2;
    //    return new Vector2(XWorld, YWorld);
    //}
}
