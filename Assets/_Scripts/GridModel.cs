using System;
using UnityEngine;

public class GridModel : MonoBehaviour
{
    [SerializeField] int width = default;
    public int Width => width;
    [SerializeField] int height = default;
    public int Height => height;

    Cell[,] grid;

    public event Action<Cell[,]> GridCreated;
    public event Action<Cell[,]> GridChanged;

    void Start()
    {
        CreateGrid();
        ChangeGrid();
    }

    void CreateGrid()
    {
        grid = new Cell[width, height];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].GridCoordinate = new Vector2Int(i, j);
                grid[i, j].Level = 0;
            }
        }
        GridCreated(grid);
    }

    void ChangeGrid()
    {
        GridChanged(grid);
    }
}