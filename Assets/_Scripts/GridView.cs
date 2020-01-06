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
        float maximumDimension = Mathf.Max(grid.GetLength(0), grid.GetLength(1));
        float playFieldWidth = 1080f;
        float reductionPercentage = 110f;
        //TODO HIGH Scale
        Scale = playFieldWidth / maximumDimension / reductionPercentage; // * FindObjectOfType<Canvas>().transform.localScale.x * 100f;
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

    void OnGridChanged(Vector2Int[] coordinates, int level)
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            if (level != 0)
            {
                cells[coordinates[i].x, coordinates[i].y].GetComponentInChildren<Text>().text = level.ToString();
                cells[coordinates[i].x, coordinates[i].y].GetComponentsInChildren<Image>()[cellTileImageIndex].color = colors.Palete[(level - 1) % colors.Palete.Length];
                cells[coordinates[i].x, coordinates[i].y].GetComponentsInChildren<Image>()[cellTileImageIndex].enabled = true;
            }
            else
            {
                cells[coordinates[i].x, coordinates[i].y].GetComponentsInChildren<Image>()[cellTileImageIndex].enabled = false;
                cells[coordinates[i].x, coordinates[i].y].GetComponentInChildren<Text>().text = "";
            }
        }
    }

    Vector2Int[] oldShadowArea;
    public void DrawShadow(Vector2Int[] area)
    {
        //Remove old shadow
        if (oldShadowArea != null)
        {
            for (int i = 0; i < oldShadowArea.Length; i++)
            {
                cells[oldShadowArea[i].x, oldShadowArea[i].y].GetComponentsInChildren<Image>()[cellShadowImageIndex].enabled = false;
            }
        }
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