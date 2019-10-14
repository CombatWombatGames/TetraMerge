using UnityEngine;

//Holds piece variants and generates random next pieces
public class PiecesCollection : MonoBehaviour
{
    Piece[] pieces = new Piece[6];
    [HideInInspector] public Piece[] NextPieces;

    void Awake()
    {
        FillCollection();
    }

    void Start()
    {
        NextPieces = GenerateRandomPieces();
    }

    void FillCollection()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = new Piece();
        }
        //O
        pieces[0].Cells = new Cell[] {
        new Cell(1, new Vector2Int(0, 0)),
        new Cell(1, new Vector2Int(1, 0)),
        new Cell(1, new Vector2Int(0, -1)),
        new Cell(1, new Vector2Int(1, -1))};
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
    }

    Piece[] GenerateRandomPieces()
    {
        return new Piece[] { pieces[Random.Range(0, pieces.Length)], pieces[Random.Range(0, pieces.Length)], pieces[Random.Range(0, pieces.Length)] };
    }
}
