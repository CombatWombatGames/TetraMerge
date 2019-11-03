using UnityEngine;

//Cell of a piece or field
public struct Cell
{
    public int Level;
    public Vector2Int GridCoordinate;

    public Cell(int Level, Vector2Int GridCoordinate)
    {
        this.Level = Level;
        this.GridCoordinate = GridCoordinate;
    }
}