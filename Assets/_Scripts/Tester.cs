using System.Collections;
using UnityEditor;
using UnityEngine;

//For dirty shortcuts during development
public class Tester : MonoBehaviour
{
#if UNITY_EDITOR
    void Start()
    {
        //StartCoroutine(LateStart(1f));
    }

    IEnumerator LateStart(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
#endif

    public void FillGrid(int level)
    {
        GridModel grid = GetComponent<GridModel>();
        int x = grid.Width;
        int y = grid.Height;
        Vector2Int[] area = new Vector2Int[x * y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                area[i + y * j] = new Vector2Int(i, j);
            }
        }
        grid.ChangeGrid(area, level);
    }

    [ContextMenu("FillGridWithBasicRunes")]
    public void FillGridWithBasicRunes()
    {
        FillGrid(GetComponent<PlayerProgressionModel>().LevelNumber);
    }

    [ContextMenu("ClearBasicRunes")]
    public void ClearBasicRunes()
    {
        GetComponent<BoostersModel>().ClearBasicRunes();
    }
}