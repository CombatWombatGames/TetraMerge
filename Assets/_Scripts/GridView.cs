using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Displays field to player
public class GridView : MonoBehaviour
{
    public float CellSize { get; private set; } //In units
    public Vector3 FingerShift { get; private set; }
    public Vector3 BoosterFingerShift { get; private set; }

    [SerializeField] GameObject cellPrefab = default;
    [SerializeField] Transform cellsParent = default;
    [SerializeField] Sprite[] tiles = default;

    GridModel gridModel;
    GameObject[,] cells;
    float fieldOffsetY;
    int cellTileImageIndex = 0;
    int cellShadowImageIndex = 1;

    void Awake()
    {
        gridModel = GetComponent<GridModel>();
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
        //Calculate scales
        float maximumDimension = Mathf.Max(grid.GetLength(0), grid.GetLength(1));
        float pixelsPerUnits = 100f;
        float scale;
        if (Screen.height / (float)Screen.width  > 16f / 9f)
        {
            scale = Screen.width / 1080f;
        }
        else
        {
            scale = Screen.height / 1920f;
        }
        fieldOffsetY = 1.3f * scale;
        float playFieldWidth = 756f * scale;
        CellSize = playFieldWidth / maximumDimension / pixelsPerUnits; //Should be equal to sprite size (126 in reference resolution)
        FingerShift = Vector3.up * CellSize * 1.5f;
        BoosterFingerShift = FingerShift * 1.5f;
        //Create empty grid
        cells = new GameObject[grid.GetLength(0), grid.GetLength(1)];
        float offsetX = (float)(grid.GetLength(0) - 1) / 2;
        float offsetY = (float)(grid.GetLength(1) - 1) / 2;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3((i - offsetX) * CellSize, (j - offsetY) * CellSize + fieldOffsetY), Quaternion.identity, cellsParent);
                AssembleCellView(cell, grid[i, j].Level);
                cells[i, j] = cell;
            }
        }
    }

    void AssembleCellView(GameObject cell, int level)
    {
        if (level != 0)
        {
            //cell.GetComponentInChildren<Text>().text = level.ToString();
            cell.GetComponentsInChildren<Image>()[cellTileImageIndex].sprite = tiles[(level - 1) % tiles.Length];
            cell.GetComponentsInChildren<Image>()[cellTileImageIndex].enabled = true;
        }
        else
        {
            cell.GetComponentsInChildren<Image>()[cellTileImageIndex].enabled = false;
            cell.GetComponentInChildren<Text>().text = "";
        }
    }

    void OnGridChanged(Vector2Int[] coordinates, int level)
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            AssembleCellView(cells[coordinates[i].x, coordinates[i].y], level);
        }
    }

    Vector2Int[] oldShadowArea;
    public void DrawShadow(Vector2Int[] area, bool sound = false)
    {
        if (sound && (oldShadowArea == null || (oldShadowArea != null && !area.SequenceEqual(oldShadowArea))))
        {
            FindObjectOfType<AudioSystem>().PlayHighlightSfx();
        }
        //Remove old shadow
        DeleteShadow();
        //Store shadowless state
        oldShadowArea = area;
        //Drop shadow
        for (int i = 0; i < area.Length; i++)
        {
            cells[oldShadowArea[i].x, oldShadowArea[i].y].GetComponentsInChildren<Image>()[cellShadowImageIndex].enabled = true;
        }
    }

    public void DeleteShadow()
    {
        if (oldShadowArea != null)
        {
            for (int i = 0; i < oldShadowArea.Length; i++)
            {
                cells[oldShadowArea[i].x, oldShadowArea[i].y].GetComponentsInChildren<Image>()[cellShadowImageIndex].enabled = false;
            }
        }
        oldShadowArea = null;
    }

    public Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    {
        float XGrid = worldCoordinate.x / CellSize + (float)(gridModel.Width - 1) / 2;
        float YGrid = (worldCoordinate.y - fieldOffsetY) / CellSize + (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }
}