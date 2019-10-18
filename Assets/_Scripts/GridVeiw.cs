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

    void OnGridChanged(Vector2Int[] coordinates, int level)
    {
        oldArea = null;
        oldText = null;
        for (int i = 0; i < coordinates.Length; i++)
        {
            cells[coordinates[i].x, coordinates[i].y].GetComponentInChildren<Text>().text = level.ToString();
        }
    }

    Vector2Int[] oldArea;
    string[] oldText;
    public void DrawPieceShadow(Vector2Int[] area)
    {
        //Remove old shadow
        if (oldArea != null)
        {
            for (int i = 0; i < oldArea.Length; i++)
            {
                cells[oldArea[i].x, oldArea[i].y].GetComponentInChildren<Text>().text = oldText[i];
            }
        }
        //Store shadowless state
        oldArea = area;
        oldText = new string[area.Length];
        for (int i = 0; i < area.Length; i++)
        {
            oldText[i] = cells[area[i].x, area[i].y].GetComponentInChildren<Text>().text;
        }
        //Drop shadow
        for (int i = 0; i < area.Length; i++)
        {
            cells[area[i].x, area[i].y].GetComponentInChildren<Text>().text = "+";
        }
    }

    public void DeletePieceShadow()
    {
        if (oldArea != null)
        {
            for (int i = 0; i < oldArea.Length; i++)
            {
                cells[oldArea[i].x, oldArea[i].y].GetComponentInChildren<Text>().text = oldText[i];
            }
        }
    }
}