//Piece inside 3x3 grid
public struct Piece
{
    //TODO LOW Piece level, which determines cells levels
    public int Identifier;
    public Cell[] Cells;
    public Piece(Piece piece)
    {
        Identifier = piece.Identifier;
        //Cloning to avoid passing all cells by reference
        //Otherwise rotation applies to every piece of its kind
        //TODO LOW Find better solution
        Cells = (Cell[])piece.Cells.Clone();
    }
    public Piece(Cell[] сells, int identifier)
    {
        Cells = сells;
        Identifier = identifier;
    }
}