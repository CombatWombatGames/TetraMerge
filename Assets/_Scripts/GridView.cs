using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
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

    Vector2Int[] oldShadowArea;
    string[] shadowlessText;
    void OnGridChanged(Vector2Int[] coordinates, int level)
    {
        //Clear stored shadowless state
        oldShadowArea = null;
        shadowlessText = null;
        //Show changes
        for (int i = 0; i < coordinates.Length; i++)
        {
            cells[coordinates[i].x, coordinates[i].y].GetComponentInChildren<Text>().text = level.ToString();
        }
    }

    public void DrawPieceShadow(Vector2Int[] area)
    {
        //Remove old shadow
        if (oldShadowArea != null)
        {
            for (int i = 0; i < oldShadowArea.Length; i++)
            {
                cells[oldShadowArea[i].x, oldShadowArea[i].y].GetComponentInChildren<Text>().text = shadowlessText[i];
            }
        }
        //Store shadowless state
        oldShadowArea = area;
        shadowlessText = new string[area.Length];
        for (int i = 0; i < area.Length; i++)
        {
            shadowlessText[i] = cells[area[i].x, area[i].y].GetComponentInChildren<Text>().text;
        }
        //Drop shadow
        for (int i = 0; i < area.Length; i++)
        {
            cells[area[i].x, area[i].y].GetComponentInChildren<Text>().text = "+";
        }
    }

    public void DeletePieceShadow()
    {
        if (oldShadowArea != null)
        {
            for (int i = 0; i < oldShadowArea.Length; i++)
            {
                cells[oldShadowArea[i].x, oldShadowArea[i].y].GetComponentInChildren<Text>().text = shadowlessText[i];
            }
        }
    }

    public void DrawSelectionShadow(Vector2Int[] area)
    {
    }
}