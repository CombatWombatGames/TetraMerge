using Array2DEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Holds types of pieces and generates random next pieces
public class PiecesModel : MonoBehaviour
{
    //TODO Rename
    [SerializeField] Array2DBool[] piecesVariants = default;

    public Piece[] NextPieces { get; private set; }
    Piece[] pieces = new Piece[6];

    public event Action PiecesGenerated;
    public event Action<int> PieceRemoved;
    public event Action PieceRotated;

    void Awake()
    {
        InitializeCollection();
    }

    void Start()
    {
        GenerateNextPieces();
    }

    void InitializeCollection()
    {
        for (int i = 0; i < piecesVariants.Length; i++)
        {
            pieces[i] = new Piece();
            pieces[i].Cells = ArrayToPiece(piecesVariants[i]);
        }
    }

    Cell[] ArrayToPiece(Array2DBool array2DBool)
    {
        List<Cell> cells = new List<Cell>();
        bool[,] arrayCells = array2DBool.GetCells();
        for (int i = 0; i < array2DBool.GridSize.x; i++)
        {
            for (int j = 0; j < array2DBool.GridSize.y; j++)
            {
                if (arrayCells[i, j])
                {
                    cells.Add(new Cell(1, new Vector2Int(j - 1, 1 - i)));
                }
            }
        }
        return cells.ToArray();
    }

    void GenerateNextPieces()
    {
        NextPieces = GenerateRandomPieces();
        //RotateAllPiecesRandom();
        PiecesGenerated();
    }

    Piece[] GenerateRandomPieces()
    {
        return new Piece[] { new Piece(pieces[Random.Range(0, pieces.Length)]), new Piece(pieces[Random.Range(0, pieces.Length)]), new Piece(pieces[Random.Range(0, pieces.Length)]) };
    }

    //TODO Proper way
    int count = 3;
    public void RemovePiece(int index)
    {
        PieceRemoved(index);
        count--;
        if (count == 0)
        {
            GenerateNextPieces();
            count = 3;
        }
    }

    Vector2Int[,] rotationLookupTable = new Vector2Int[,] {
        { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1) },
        { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) },
        { new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1) } };
    public void RotatePiece(int index)
    {
        for (int i = 0; i < NextPieces[index].Cells.Length; i++)
        {
            NextPieces[index].Cells[i].GridCoordinate = rotationLookupTable[NextPieces[index].Cells[i].GridCoordinate.x + 1, NextPieces[index].Cells[i].GridCoordinate.y + 1];
        }
        PieceRotated();
    }

    void RotateAllPiecesRandom()
    {
        for (int i = 0; i < NextPieces.Length; i++)
        {
            int rotations = Random.Range(0, 4);
            for (int j = 1; j <= rotations; j++)
            {
                RotatePiece(i);
            }
        }
    }
}