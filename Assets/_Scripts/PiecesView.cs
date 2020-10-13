using UnityEngine;
using UnityEngine.UI;

//Displays pieces to player
public class PiecesView : MonoBehaviour
{
    [SerializeField] PieceController[] nextPieces = default;
    [SerializeField] Sprite[] tiles = default;

    PiecesModel piecesModel;
    Image[][] nextPiecesImages = new Image[3][];

    void Awake()
    {
        piecesModel = GetComponent<PiecesModel>();
        piecesModel.PieceRemoved += HidePiece;
        piecesModel.PiecesGenerated += ShowPieces;
        piecesModel.PieceRotated += ShowPieceByIndex;
        piecesModel.CollectionLevelUp += ShowPieces;
        InitializeImages();
    }

    void OnDestroy()
    {
        piecesModel.PieceRemoved -= HidePiece;
        piecesModel.PiecesGenerated -= ShowPieces;
        piecesModel.PieceRotated -= ShowPieceByIndex;
        piecesModel.CollectionLevelUp -= ShowPieces;
    }

    void InitializeImages()
    {
        for (int i = 0; i < nextPieces.Length; i++)
        {
            nextPiecesImages[i] = nextPieces[i].GetComponentsInChildren<Image>();
        }
    }

    void ShowPieces()
    {
        for (int i = 0; i < nextPieces.Length; i++)
        {
            ShowPiece(piecesModel.NextPieces[i], nextPiecesImages[i]);
        }
    }

    void ShowPieceByIndex(int index)
    {
        ShowPiece(piecesModel.NextPieces[index], nextPiecesImages[index]);
    }

    void ShowPiece(Piece piece, Image[] slot)
    {
        if (piece.Cells.Length != 0)
        {
            bool[] mask = PieceToMask(piece);
            for (int i = 0; i < 9; i++)
            {
                //TODO LOW Fix duplicating functionality
                slot[i].sprite = tiles[(piece.Cells[0].Level - 1) % tiles.Length];
                slot[i].enabled = mask[i];
            }
        }
    }

    bool[] PieceToMask(Piece piece)
    {
        bool[] mask = new bool[9];
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            mask[(1 - piece.Cells[i].GridCoordinate.y) * 3 + piece.Cells[i].GridCoordinate.x + 1] = true;
        }
        return mask;
    }

    public void ReturnPiece(int index)
    {
        nextPieces[index].transform.localPosition = Vector3.zero;
    }

    void HidePiece(int index)
    {
        nextPieces[index].transform.localPosition = Vector3.zero;
    }

    public void ScalePiece(int index, float scale)
    {
        nextPieces[index].transform.localScale *= scale;
    }
}
