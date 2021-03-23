using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [SerializeField] Image background = default;
    [SerializeField] Transform panel = default;
    [SerializeField] GameObject canvas = default;

    public Image Background => background;
    public Transform Panel => panel;
    public GameObject Canvas => canvas;
}
