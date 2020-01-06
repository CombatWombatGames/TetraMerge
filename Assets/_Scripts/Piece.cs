//Piece inside 3x3 grid
public struct Piece
{
    public int Identifier;
    public Cell[] Cells;
    public Piece(Piece piece)
    {
        Identifier = piece.Identifier;
        //Copying cell by cell to avoid passing all cells by reference
        //Otherwise rotation applies to every piece of its kind
        //TODO LOW Find better solution
        Cells = new Cell[piece.Cells.Length];
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Cells[i] = piece.Cells[i];
        }
    }
    public Piece(Cell[] сells, int identifier)
    {
        Cells = сells;
        Identifier = identifier;
    }
}
