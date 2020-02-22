using System;
using UnityEngine;

//Holds state of the field and provides ways to change it
[DefaultExecutionOrder(-1)]
public class GridModel : MonoBehaviour
{
    public event Action<Cell[,]> GridCreated;
    public event Action<Vector2Int[], int> GridChanged;

    public int Width => width;
    public int Height => height;
    public Cell[,] Grid { get; private set; }

    [SerializeField] int height = default;
    [SerializeField] int width = default;
    [SerializeField] PiecesModel piecesModel = default;
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;

    public void Initialize(int[] grid)
    {
        Grid = IntegersToCells(grid);
        GridCreated(Grid);
    }

    Cell[,] IntegersToCells(int[] integers)
    {
        Cell[,] grid = new Cell[width, height];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].GridCoordinate = new Vector2Int(i, j);
                grid[i, grid.GetLength(1) - j - 1].Level = integers[i + j * grid.GetLength(0)];
            }
        }
        return grid;
    }

    void CreateEmptyGrid()
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
        //Happens after merge or clear booster only
        if (level == 0)
        {
            while (MinimumLevelPiecesRemoved())
            {
                playerProgressionModel.LevelNumber++;
                piecesModel.LevelUpCollection();
            }
        }
    }

    bool MinimumLevelPiecesRemoved()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if (Grid[i, j].Level == playerProgressionModel.LevelNumber)
                {
                    return false;
                }
            }
        }
        return true;
    }
}