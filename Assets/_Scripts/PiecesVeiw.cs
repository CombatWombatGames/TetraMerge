using UnityEngine;
using UnityEngine.UI;

//Displays game state to player
public class PiecesVeiw : MonoBehaviour
{
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] PieceController[] nextPieces = default;
    Image[][] nextPiecesImages = new Image[3][];

    void Awake()
    {
        piecesModel.PieceRemoved += HidePiece;
        piecesModel.PiecesGenerated += ShowPieces;
        InitializeImages();
    }

    void OnDestroy()
    {
        piecesModel.PieceRemoved -= HidePiece;
        piecesModel.PiecesGenerated -= ShowPieces;
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

    void ShowPiece(Piece piece, Image[] slot)
    {
        bool[] mask = PieceToMask(piece);
        for (int i = 0; i < 9; i++)
        {
            slot[i].enabled = mask[i];
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
        ShowPiece(piecesModel.NextPieces[index], nextPiecesImages[index]);
        nextPieces[index].transform.localPosition = Vector3.zero;
    }
}
