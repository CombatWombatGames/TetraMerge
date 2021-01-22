using Array2DEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

//Holds state of the field and provides ways to change it
public class GridModel : MonoBehaviour
{
    public event Action<Cell[,]> GridCreated;
    public event Action<Vector2Int[], int> GridChanged;
    public event Action<int> CellsMerged;

    public int Width { get; } = 6;
    public int Height { get; } = 6;
    public Cell[,] Grid { get; private set; }

    PiecesModel piecesModel;
    BoostersModel boostersModel;
    PlayerProgressionModel playerProgressionModel;

    public void Initialize(int[] grid)
    {
        piecesModel = GetComponent<PiecesModel>();
        boostersModel = GetComponent<BoostersModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        Grid = IntegersToCells(grid);
        GridCreated(Grid);
    }

    Cell[,] IntegersToCells(int[] integers)
    {
        Cell[,] grid = new Cell[Width, Height];
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

    public void ChangeGrid(Vector2Int[] coordinates, int level, bool cellsMerged = true)
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            Grid[coordinates[i].x, coordinates[i].y].Level = level;
        }
        GridChanged(coordinates, level);
        //Happens after merge or clear booster or ultimate booster
        if (level == 0)
        {
            //TODO LOW Increase level repeatedly if needed
            if (MinimumLevelPiecesRemoved())
            {
                playerProgressionModel.LevelNumber++;
                piecesModel.LevelUpCollection();
            }
            if (cellsMerged)
            {
                CellsMerged(coordinates.Length);
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

    public int CountEmptyCells()
    {
        int count = 0;
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if (Grid[i, j].Level == 0)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void RemoveMinimumLevelPieces()
    {
        List<Vector2Int> coordinates = new List<Vector2Int>();
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if (Grid[i, j].Level == playerProgressionModel.LevelNumber)
                {
                    coordinates.Add(new Vector2Int(i, j));
                }
            }
        }
        ChangeGrid(coordinates.ToArray(), 0, false);
    }

    public void StageComplete()
    {
        //TODO LOW make event
        var stagesList = GetComponent<Resources>().StagesList;
        ChangeGrid(ArrayToField(stagesList[playerProgressionModel.Stage]), playerProgressionModel.LevelNumber, false);
        playerProgressionModel.Stage = (playerProgressionModel.Stage + 1) % stagesList.Length;
        boostersModel.UltimateUsed = false;
    }

    Vector2Int[] ArrayToField(Array2DBool array2DBool)
    {
        List<Vector2Int> field = new List<Vector2Int>();
        bool[,] arrayCells = array2DBool.GetCells();
        for (int i = 0; i < array2DBool.GridSize.x; i++)
        {
            for (int j = 0; j < array2DBool.GridSize.y; j++)
            {
                if (arrayCells[i, j])
                {
                    field.Add(new Vector2Int(j, Height - 1 - i));
                }
            }
        }
        return field.ToArray();
    }
}