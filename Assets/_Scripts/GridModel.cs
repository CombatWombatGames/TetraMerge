using System;
using UnityEngine;

public class GridModel : MonoBehaviour
{
    [SerializeField] int width = default;
    public int Width => width;
    [SerializeField] int height = default;
    public int Height => height;

    public Cell[,] Grid { get; private set; }

    public event Action<Cell[,]> GridCreated;
    public event Action<Vector2Int[], int> GridChanged;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        Grid = new Cell[width, height];
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Grid[i, j].GridCoordinate = new Vector2Int(i, j);
                Grid[i, j].Level = 0;
            }
        }
        GridCreated(Grid);
    }

    public void ChangeGrid(Vector2Int[] coordinates, int level)
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            Grid[coordinates[i].x, coordinates[i].y].Level = level;
        }
        GridChanged(coordinates, level);
    }
}