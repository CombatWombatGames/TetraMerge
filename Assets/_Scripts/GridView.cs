using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Displays field to player
public class GridView : MonoBehaviour
{
    public float CellSize { get; private set; } //In units
    public Vector3 FingerShift { get; private set; }
    public Vector3 BoosterFingerShift { get; private set; }
    public Transform Field => field;
    public ParticleSystem DustParticles => dustParticles;
    public ParticleSystem ShardsParticles => shardsParticles;
    public ParticleSystem LeafParticles => leafParticles;
    public ParticleSystem LeafParticlesBurst => leafParticlesBurst;
    public Image[] Vines => vines;

    [SerializeField] GameObject cellPrefab = default;
    [SerializeField] Transform cellsParent = default;
    [SerializeField] Transform field = default;
    [SerializeField] ParticleSystem dustParticles = default;
    [SerializeField] ParticleSystem shardsParticles = default;
    [SerializeField] ParticleSystem leafParticles = default;
    [SerializeField] ParticleSystem leafParticlesBurst = default;
    [SerializeField] Image[] vines = default;

    GridModel gridModel;
    GameObject[,] cells;
    float fieldOffsetY;
    int cellTileImageIndex = 0;
    int cellShadowImageIndex = 1;
    Sprite[] tiles;

    void Awake()
    {
        tiles = GetComponent<Resources>().TilesList;
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
        //Spawn tiles in random order
        var rngTable = new SortedDictionary<float, (int, int)>();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                rngTable.Add(Random.value, (i, j));
            }
        }
        AnimationSystem.FallDelay = 0f;
        int runesDropped = 0;
        foreach (var kvp in rngTable)
        {
            int i = kvp.Value.Item1;
            int j = kvp.Value.Item2;
            GameObject cell = Instantiate(cellPrefab, new Vector3((i - offsetX) * CellSize, (j - offsetY) * CellSize + fieldOffsetY), Quaternion.identity, cellsParent);
            AssembleCellView(cell, grid[i, j].Level, false, true);
            cells[i, j] = cell;
            if (grid[i, j].Level > 0)
            {
                runesDropped++;
            }
        }
        if (runesDropped > 0)
        {
            AudioSystem.Player.PlayDropSfx();
        }
        else
        {
            AudioSystem.Player.PlayBoosterSfx();
        }
        AnimationSystem.ShakeField(Field, grid.GetLength(0) * grid.GetLength(1) - gridModel.CountEmptyCells(), DustParticles, ShardsParticles, LeafParticles, LeafParticlesBurst, 0.2f);
        AnimationSystem.GrowVines(vines);
    }

    Dictionary<GameObject, Sequence> destroyngTiles = new Dictionary<GameObject, Sequence>();
    void AssembleCellView(GameObject cell, int level,  bool animateDestroy, bool animateFall)
    {
        var mainImage = cell.GetComponentsInChildren<Image>()[cellTileImageIndex];
        var glowImage = cell.GetComponentsInChildren<Image>()[cellTileImageIndex + 2];
        if (level != 0)
        {
            if (destroyngTiles.TryGetValue(cell, out var sequence))
            {
                sequence.Complete(true);
            }
            mainImage.sprite = tiles[(level - 1) % tiles.Length];
            if (animateFall)
            {
                AnimationSystem.SpawnTile(mainImage);
            }
            else
            {
                mainImage.enabled = true;
            }
            AnimationSystem.Glow(glowImage);
        }
        else if (animateDestroy)
        {
            destroyngTiles[cell] = AnimationSystem.DestroyTile(mainImage);
            AnimationSystem.StopGlow(glowImage);
        }
        else
        {
            cell.GetComponentsInChildren<Image>()[cellTileImageIndex].enabled = false;
        }
    }

    void OnGridChanged(Vector2Int[] coordinates, int level)
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            AssembleCellView(cells[coordinates[i].x, coordinates[i].y], level, true, false);
        }
        AnimationSystem.HideVines(vines);
    }

    Vector2Int[] oldShadowArea;
    public void DrawShadow(Vector2Int[] area, bool sound = false)
    {
        if (sound && (oldShadowArea == null || (oldShadowArea != null && !area.SequenceEqual(oldShadowArea))))
        {
            AudioSystem.Player.PlayHighlightSfx();
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