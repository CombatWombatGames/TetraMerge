using System;
using UnityEngine;

public class GridModel : MonoBehaviour
{
    [SerializeField] int width = default;
    [SerializeField] int height = default;

    Cell[,] grid;

    public event Action<Cell[,]> GridCreated;
    public event Action<Cell[,]> GridChanged;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Cell[width, height];
        GridCreated(grid);
    }

    void ChangeGrid()
    {
        GridChanged(grid);
    }
}