using System;
using UnityEngine;
using UnityEngine.UI;

public class PiecesVeiw : MonoBehaviour
{
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] PieceController[] nextPieces = default;
    Image[] leftPieceCells;
    Image[] middlePieceCells;
    Image[] rightPieceCells;
    Image[][] nextPiecesImages = new Image[3][];

    void Awake()
    {
        piecesModel.PieceRemoved += RemovePiece;
    }

    void OnDestroy()
    {
        piecesModel.PieceRemoved -= RemovePiece;
    }

    void Start()
    {
        leftPieceCells = nextPieces[0].GetComponentsInChildren<Image>();
        middlePieceCells = nextPieces[1].GetComponentsInChildren<Image>();
        rightPieceCells = nextPieces[2].GetComponentsInChildren<Image>();
        nextPiecesImages[0] = leftPieceCells;
        nextPiecesImages[1] = middlePieceCells;
        nextPiecesImages[2] = rightPieceCells;
        ShowPiece(piecesModel.NextPieces[0], leftPieceCells);
        ShowPiece(piecesModel.NextPieces[1], middlePieceCells);
        ShowPiece(piecesModel.NextPieces[2], rightPieceCells);
    }

    void ShowPiece(Piece piece, Image[] slot)
    {
        bool[] mask = PieceToMask(piece);
        for (int i = 0; i < 9; i++)
        {
            slot[i].enabled = mask[i];
        }
    }

    bool[] mask = new bool[9];
    bool[] PieceToMask(Piece piece)
    {
        Array.Clear(mask, 0, 9);
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

    void RemovePiece(int index)
    {
        //TODO Change order
        nextPieces[index].transform.localPosition = Vector3.zero;
        ShowPiece(piecesModel.EmptyPiece, nextPiecesImages[index]);
    }
}
