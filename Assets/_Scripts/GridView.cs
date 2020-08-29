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
    [SerializeField] Colors colors = default;

    GameObject[,] cells;
    int cellTileImageIndex = 1;
    int cellShadowImageIndex = 2;

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
        //Create empty grid
        float maximumDimension = Mathf.Max(grid.GetLength(0), grid.GetLength(1));
        float playFieldWidth = 1080f;
        float reductionPercentage = 110f;
        //TODO HIGH Scale
        Scale = playFieldWidth / maximumDimension / reductionPercentage;
        cells = new GameObject[grid.GetLength(0), grid.GetLength(1)];
        float offsetX = (float)(grid.GetLength(0) - 1) / 2;
        float offsetY = (float)(grid.GetLength(1) - 1) / 2;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3((i - offsetX) * Scale, (j - offsetY) * Scale), Quaternion.identity, cellsParent);
                cell.transform.localScale = new Vector3(Scale, Scale);
                AssembleCellView(cell, grid[i, j].Level);
                cells[i, j] = cell;
            }
        }
    }

    void AssembleCellView(GameObject cell, int level)
    {
        if (level != 0)
        {
            cell.GetComponentInChildren<Text>().text = level.ToString();
            cell.GetComponentsInChildren<Image>()[cellTileImageIndex].color = colors.Palete[(level - 1) % colors.Palete.Length];
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
    public void DrawShadow(Vector2Int[] area)
    {
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
    }
}