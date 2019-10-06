using System;
using UnityEngine;

//Holds piece variants and generates random next pieces
public class PiecesCollection : MonoBehaviour
{
    public Piece[] Pieces = new Piece[6];

    void Awake()
    {
        FillCollection();
        //GenerateRandomPieces();
    }

    void FillCollection()
    {
        for (int i = 0; i < Pieces.Length; i++)
        {
            Pieces[i] = new Piece();
        }
        //O
        Pieces[0].Cells = new Cell[] {
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(1, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
        //Z
        Pieces[1].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, 0)),
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
        //L
        Pieces[2].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1)),
        new Cell(1, new Vector2Int(1, 0))};
        //T
        Pieces[3].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
        //J
        Pieces[4].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, 0)),
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
        //S
        Pieces[5].Cells = new Cell[] {
        new Cell(1, new Vector2Int(-1, -1)),
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, 0))};
    }

    void GenerateRandomPieces()
    {
        throw new NotImplementedException();
    }
}
