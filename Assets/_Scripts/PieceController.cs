using UnityEngine;
using UnityEngine.EventSystems;

//Translates movement, rotation and dropping of the piece to models
public class PieceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    GridModel gridModel;
    GridView gridView;
    PiecesView piecesView;
    PiecesModel piecesModel;
    PlayerProgressionModel playerProgressionModel;
    int index;
    float scaleRate = 1.75f;

    public void Initialize(int index, GameObject managersContainer)
    {
        this.index = index;
        gridModel = managersContainer.GetComponent<GridModel>();
        gridView = managersContainer.GetComponent<GridView>();
        piecesView = managersContainer.GetComponent<PiecesView>();
        piecesModel = managersContainer.GetComponent<PiecesModel>();
        playerProgressionModel = managersContainer.GetComponent<PlayerProgressionModel>();
        piecesModel.PiecesGenerated += ActivatePiece;
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    public void Unsubscribe()
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
            transform.position = eventData.pointerCurrentRaycast.worldPosition + gridView.FingerShift;
            Vector2Int[] nearestArea = FindNearestArea(eventData.pointerCurrentRaycast.worldPosition + gridView.FingerShift, piecesModel.NextPieces[index]);
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
        AnimationSystem.FinishPieceRotation();
        piecesView.ScalePiece(index, scaleRate);
        AudioSystem.Player.PlayRaiseSfx();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        piecesView.ScalePiece(index, 1.0f / scaleRate);
        Vector2Int[] nearestArea = FindNearestArea(eventData.pointerCurrentRaycast.worldPosition + gridView.FingerShift, piecesModel.NextPieces[index]);
        if (AreaIsAvailable(nearestArea) && eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            //Drop the piece
            //TODO LOW Ask for level another way. MB make entire piece level?
            gridModel.ChangeGrid(nearestArea, piecesModel.NextPieces[index].Cells[0].Level);
            gameObject.SetActive(false);
            piecesModel.RemovePiece(index);
            playerProgressionModel.TurnNumber++;
            AudioSystem.Player.PlayDropSfx();
        }
        else
        {
            piecesView.ReturnPiece(index);
            AudioSystem.Player.PlayTurnSfx();
        }
        gridView.DeleteShadow();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            AudioSystem.Player.PlayTurnSfx();
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
