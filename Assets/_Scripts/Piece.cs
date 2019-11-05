//Piece inside 3x3 grid
public struct Piece
{
    public Cell[] Cells;
    public Piece(Piece piece)
    {
        //Copying cell by cell to avoid passing all cells by reference
        //Otherwise rotation applies to every piece of its kind
        //TODO Find better solution
        Cells = new Cell[piece.Cells.Length];
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Cells[i] = piece.Cells[i];
        }
    }
    public Piece(Cell[] сells)
    {
        Cells = сells;
    }
}
