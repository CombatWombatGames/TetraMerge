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
}