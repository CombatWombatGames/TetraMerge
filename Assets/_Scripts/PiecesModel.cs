using Array2DEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Holds types of pieces and generates random next pieces
public class PiecesModel : MonoBehaviour
{
    public event Action PiecesGenerated;
    public event Action<int> PieceRemoved;
    public event Action<int> PieceRotated;
    public event Action CollectionLevelUp;
    public Piece[] Pieces { get; private set; }
    public Piece[] NextPieces { get; private set; }

    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    [SerializeField] Array2DBool[] piecesVariants = default;

    Piece emptyPiece = new Piece(new Cell[0], -1);

    public void Initialize(int[] nextPieces, int levelNumber)
    {
        InitializeCollection(levelNumber);
        NextPieces = IntegersToPieces(nextPieces);
        PiecesGenerated();
    }

    void InitializeCollection(int levelNumber)
    {
        Pieces = new Piece[piecesVariants.Length];
        for (int i = 0; i < piecesVariants.Length; i++)
        {
            Pieces[i] = new Piece(ArrayToPiece(piecesVariants[i]), i);
            for (int j = 0; j < Pieces[i].Cells.Length; j++)
            {
                Pieces[i].Cells[j].Level = levelNumber;
            }
        }
    }

    Piece[] IntegersToPieces(int[] integers)
    {
        Piece[] pieces = new Piece[integers.Length];
        for (int i = 0; i < pieces.Length; i++)
        {
            if (integers[i] != -1)
            {
                pieces[i] = new Piece(Pieces[integers[i]]);
            }
            else
            {
                pieces[i] = new Piece(emptyPiece);
            }
        }
        return pieces;
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

    public void GenerateNextPieces()
    {
        NextPieces = GenerateRandomPieces();
        //TODO LOW Put in setter?
        PiecesGenerated();
    }

    Piece[] GenerateRandomPieces()
    {
        if (playerProgressionModel.TurnNumber != 0)
        {
            return new Piece[] { new Piece(Pieces[Random.Range(0, Pieces.Length)]), new Piece(Pieces[Random.Range(0, Pieces.Length)]), new Piece(Pieces[Random.Range(0, Pieces.Length)]) };
        }
        else
        {
            //Do not spawn "O"-figures at the first turn (mb also forbid 2J/2L?)
            return new Piece[] { new Piece(Pieces[Random.Range(1, Pieces.Length)]), new Piece(Pieces[Random.Range(1, Pieces.Length)]), new Piece(Pieces[Random.Range(1, Pieces.Length)]) };
        }
    }

    public void RemovePiece(int index)
    {
        NextPieces[index] = emptyPiece;
        PieceRemoved(index);
        //Check if it was the last piece
        for (int i = 0; i < NextPieces.Length; i++)
        {
            if (NextPieces[i].Cells.Length != 0)
            {
                return;
            }
            else if (i == NextPieces.Length - 1)
            {
                GenerateNextPieces();
            }
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
        PieceRotated(index);
    }

    //TODO HIGH Make set level function and call from here and from init
    public void LevelUpCollection()
    {
        //Update collection
        for (int i = 0; i < Pieces.Length; i++)
        {
            LevelUpPiece(Pieces[i]);
        }
        //Update pieces already generated
        for (int i = 0; i < NextPieces.Length; i++)
        {
            LevelUpPiece(NextPieces[i]);
        }
        CollectionLevelUp();
    }

    void LevelUpPiece(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            piece.Cells[i].Level++;
        }
    }
}