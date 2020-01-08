using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        FillGrid(0);
        FindObjectOfType<PlayerProgressionModel>().TurnNumber = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FillGrid(int level)
    {
        int x = FindObjectOfType<GridModel>().Width;
        int y = FindObjectOfType<GridModel>().Height;
        Vector2Int[] area = new Vector2Int[x * y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                area[i + y * j] = new Vector2Int(i, j);
            }
        }
        FindObjectOfType<GridModel>().ChangeGrid(area, level);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        Debug.Log("Quit!");
#endif
        Application.Quit();
    }
}