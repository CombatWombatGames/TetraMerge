using UnityEngine;

public class GridVeiw : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab = default;
    [SerializeField] GridModel gridModel = default;
    [SerializeField] Transform cellsParent = default;

    void Awake()
    {
        gridModel.GridCreated += OnGridCreated;
    }

    void OnDestroy()
    {
        gridModel.GridCreated -= OnGridCreated;
    }

    void OnGridCreated(Cell[,] grid)
    {
        float offsetX = (float)(grid.GetLength(0) - 1) / 2;
        float offsetY = (float)(grid.GetLength(1) - 1) / 2;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Instantiate(cellPrefab, new Vector3(i - offsetX, j - offsetY), Quaternion.identity, cellsParent);
            }
        }
    }
}
