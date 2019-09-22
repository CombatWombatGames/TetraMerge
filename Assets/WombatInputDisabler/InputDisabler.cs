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
}
