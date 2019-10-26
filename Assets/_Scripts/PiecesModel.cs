﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

//Holds piece variants and generates random next pieces
public class PiecesModel : MonoBehaviour
{
    public Piece[] NextPieces { get; private set; }
    Piece[] pieces = new Piece[7];

    public event Action PiecesGenerated;
    public event Action<int> PieceRemoved;
    public event Action PieceRotated;

    void Awake()
    {
        FillCollection();
    }

    void Start()
    {
        GenerateNextPieces();
    }

    void GenerateNextPieces()
    {
        NextPieces = GenerateRandomPieces();
        //RotateAllPiecesRandom();
        PiecesGenerated();
    }

    void FillCollection()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = new Piece();
        }
        //Empty
        pieces[0].Cells = new Cell[0];
        //Z
        pieces[1].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, 0)),
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
        //L
        pieces[2].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1)),
        new Cell(1, new Vector2Int(1, 0))};
        //T
        pieces[3].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
        //J
        pieces[4].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, 0)),
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
        //S
        pieces[5].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, 0))};
        //O
        pieces[6].Cells = new Cell[] {
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(1, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
    }

    Piece[] GenerateRandomPieces()
    {
        return new Piece[] { new Piece(pieces[Random.Range(1, pieces.Length)]), new Piece(pieces[Random.Range(1, pieces.Length)]), new Piece(pieces[Random.Range(1, pieces.Length)]) };
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
