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

    //UGUI
    public void RestartScene()
    {
        FindObjectOfType<PlayerProgressionModel>().UpdateBestScore();
        FindObjectOfType<SaveSystem>().StartFromScratch();
    }

    //UGUI
    public void FillGrid(int level)
    {
        GridModel grid = FindObjectOfType<GridModel>();
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

    //UGUI
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}