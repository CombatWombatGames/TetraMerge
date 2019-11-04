using UnityEngine;
using UnityEngine.UI;

//Displays field to player
public class GridView : MonoBehaviour
{
    //Used in cell instantiation scaling, conversions between world and grid coordinates
    public float Scale { get; private set; }

    [SerializeField] GridModel gridModel = default;
    [SerializeField] GameObject cellPrefab = default;
    [SerializeField] Transform cellsParent = default;

    GameObject[,] cells;
    Color[] colors = new Color[] { Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow, Color.blue };

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
        float maximumDimension = Mathf.Max(grid.GetLength(0), grid.GetLength(1));
        float screenWidth = 1080;
        float reductionPercentage = 110;
        Scale = screenWidth / maximumDimension / reductionPercentage;
        cells = new GameObject[grid.GetLength(0), grid.GetLength(1)];
        float offsetX = (float)(grid.GetLength(0) - 1) / 2;
        float offsetY = (float)(grid.GetLength(1) - 1) / 2;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3((i - offsetX) * Scale, (j - offsetY) * Scale), Quaternion.identity, cellsParent);
                cell.transform.localScale = new Vector3(Scale, Scale);
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
            cells[coordinates[i].x, coordinates[i].y].GetComponentInChildren<Text>().color = colors[level % colors.Length];
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
        DrawPieceShadow(area);
    }

    public void DeleteSelectionShadow()
    {
        DeletePieceShadow();
    }
}