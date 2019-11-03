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

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FillGridWithOnes()
    {
        Vector2Int[] ones = new Vector2Int[100];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                ones[i + 10 * j] = new Vector2Int(i, j);
            }
        }
        FindObjectOfType<GridModel>().ChangeGrid(ones, 1);
    }
}