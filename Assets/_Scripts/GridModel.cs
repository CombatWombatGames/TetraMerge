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
    public event Action<Cell[,]> GridChanged;

    void Start()
    {
        CreateGrid();
        ChangeGrid();
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

    void ChangeGrid()
    {
        GridChanged(Grid);
    }

    public void DropPiece(Vector2Int[] area)
    {
        //void DeletePiece
        for (int i = 0; i < area.Length; i++)
        {
            //cells[area[i].x, area[i].y].GetComponentInChildren<Text>().text = "1";
        }
    }
}