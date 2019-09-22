using UnityEngine;

public class GameViewScale : MonoBehaviour
{
#if UNITY_EDITOR
    void Awake()
    {
        SetGameViewScale();
    }

    void SetGameViewScale()
    {
        System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
        System.Type type = assembly.GetType("UnityEditor.GameView");
        UnityEditor.EditorWindow v = UnityEditor.EditorWindow.GetWindow(type);
        var defScaleField = type.GetField("m_defaultScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //Scale on click Play
        float defaultScale = 0.33f;
        var areaField = type.GetField("m_ZoomArea", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var areaObject = areaField.GetValue(v);
        var scaleField = areaObject.GetType().GetField("m_Scale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        scaleField.SetValue(areaObject, new Vector2(defaultScale, defaultScale));
    }
#endif
}