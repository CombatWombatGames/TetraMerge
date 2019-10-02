using UnityEngine;

//Тут будет пользовательский ввод менять модель
public class GridController : MonoBehaviour
{
    [SerializeField] GridModel gridModel = default;

    Vector2Int WorldToGridCoordinate(Vector2 worldCoordinate)
    {
        float XGrid = worldCoordinate.x - (float)(gridModel.Width - 1) / 2;
        float YGrid = worldCoordinate.y - (float)(gridModel.Height - 1) / 2;
        return new Vector2Int(Mathf.RoundToInt(XGrid), Mathf.RoundToInt(YGrid));
    }

    Vector2 GridToWorldCoordinate(Vector2Int gridCoordinate)
    {
        float XWorld = gridCoordinate.x + (float)(gridModel.Width - 1) / 2;
        float YWorld = gridCoordinate.y + (float)(gridModel.Height - 1) / 2;
        return new Vector2(XWorld, YWorld);
    }
}
