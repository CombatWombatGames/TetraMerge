using System.Collections;
using UnityEngine;

public class InputDisabler : MonoBehaviour
{
    [SerializeField] GameObject BlockingCanvas = default;

    public void DisableInput()
    {
        BlockingCanvas.SetActive(true);
    }

    public void EnableInput()
    {
        BlockingCanvas.SetActive(false);
    }

    public void DisableInput(float seconds)
    {
        StartCoroutine(ActivateBlocker(seconds));
    }

    IEnumerator ActivateBlocker(float seconds)
    {
        BlockingCanvas.SetActive(true);
        yield return new WaitForSeconds(seconds);
        BlockingCanvas.SetActive(false);
    }
}
