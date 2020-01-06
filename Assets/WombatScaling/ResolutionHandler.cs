using UnityEngine;
using UnityEngine.UI;

public class ResolutionHandler : MonoBehaviour
{
    float nineBySixteen = 0.5625f;

    void Awake()
    {
        ChangeScaling();
    }

    void ChangeScaling()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        float aspectRatio = (float)Screen.width / Screen.height;
        if (aspectRatio >= nineBySixteen)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }

#if UNITY_EDITOR
    Vector2 resolution;

    void Start()
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
}