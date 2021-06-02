using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture = default;
    [SerializeField] Texture2D transparentTexture = default;
    CursorMode cursorMode = CursorMode.ForceSoftware;
    Vector2 hotSpot = new Vector2(23f, 25f);

    void Start()
    {
        Cursor.SetCursor(transparentTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(transparentTexture, hotSpot, cursorMode);
        }
    }
}
