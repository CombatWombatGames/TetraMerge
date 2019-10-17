using UnityEngine;
using UnityEngine.UI;

public class GridVeiw : MonoBehaviour
{
    [SerializeField] GridModel gridModel = default;
    [SerializeField] GameObject cellPrefab = default;
    [SerializeField] Transform cellsParent = default;
    GameObject[,] cells;

    void Awake()
    {
        gridModel.GridCreated += OnGridCreated;
        gridModel.GridChanged += OnGridChanged;
    }

    void OnDestroy()
    {
        gridModel.GridCreated -= OnGridCreated;
        gridModel.GridChanged -= OnGridChanged;
    }

    void OnGridCreated(Cell[,] grid)
    {
        cells = new GameObject[grid.GetLength(0), grid.GetLength(1)];
        float offsetX = (float)(grid.GetLength(0) - 1) / 2;
        float offsetY = (float)(grid.GetLength(1) - 1) / 2;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(i - offsetX, j - offsetY), Quaternion.identity, cellsParent);
                cells[i, j] = cell;
            }
        }
    }

    void OnGridChanged(Cell[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                cells[i, j].GetComponentInChildren<Text>().text = $"{grid[i, j].GridCoordinate.x},{grid[i, j].GridCoordinate.y}:{grid[i, j].Level}";
            }
        }
    }

    public void DrawPieceShadow(Vector2Int[] area)
    {
        OnGridChanged(gridModel.Grid);
        for (int i = 0; i < area.Length; i++)
        {
            cells[area[i].x, area[i].y].GetComponentInChildren<Text>().text = "1";
        }
    }

    public void DeletePieceShadow()
    {
        OnGridChanged(gridModel.Grid);
    }

    public void DropPiece(Vector2Int[] area)
    {
        for (int i = 0; i < area.Length; i++)
        {
            cells[area[i].x, area[i].y].GetComponentInChildren<Text>().text = "1";
        }
    }
}