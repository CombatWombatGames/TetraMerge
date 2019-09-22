using UnityEngine;
using UnityEngine.UI;

public class ResolutionHandler : MonoBehaviour
{
#if UNITY_EDITOR
    Vector2 resolution;

    void Awake()
    {
        resolution = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            ChangeScaling();
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }
#endif

    void Start()
    {
        ChangeScaling();
    }

    void ChangeScaling()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        float aspectRatio = (float)Screen.width / Screen.height;
        if (aspectRatio >= 0.5625)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }
}