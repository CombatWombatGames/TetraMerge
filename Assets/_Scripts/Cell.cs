using UnityEngine;

//Cell of a piece or field
public struct Cell
{
    public int Level;
    public Vector2Int GridCoordinate;

    public Cell(int level, Vector2Int gridCoordinate)
    {
        Level = level;
        GridCoordinate = gridCoordinate;
    }
}