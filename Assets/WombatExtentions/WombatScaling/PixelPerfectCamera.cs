using UnityEngine;

namespace WombatScaling
{
    public class PixelPerfectCamera : MonoBehaviour
    {
        int targetPixelsPerUnit = 100;

        void Awake()
        {
            AdjustCameraSize();
        }

        private void AdjustCameraSize()
        {
            float halfScreenInUnits = (float)Screen.height / targetPixelsPerUnit / 2;
            GetComponent<Camera>().orthographicSize = halfScreenInUnits;
        }

#if UNITY_EDITOR
        int screenHeight;

        void Start()
        {
            screenHeight = Screen.height;
        }

        void Update()
        {
            if (screenHeight != Screen.height)
            {
                AdjustCameraSize();
                screenHeight = Screen.height;
            }
        }
#endif
    }
}