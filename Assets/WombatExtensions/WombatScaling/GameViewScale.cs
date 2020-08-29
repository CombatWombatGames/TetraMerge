using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;

namespace WombatScaling
{
    public class GameViewScale : MonoBehaviour
    {
#if UNITY_EDITOR
        float desiredGameViewScale = 0.33f;

        void Awake()
        {
            SetGameViewScale();
        }

        void SetGameViewScale()
        {
            Assembly assembly = typeof(EditorWindow).Assembly;
            Type type = assembly.GetType("UnityEditor.GameView");
            EditorWindow window = EditorWindow.GetWindow(type);
            var defScaleField = type.GetField("m_defaultScale", BindingFlags.Instance | BindingFlags.NonPublic);
            var areaField = type.GetField("m_ZoomArea", BindingFlags.Instance | BindingFlags.NonPublic);
            var areaObject = areaField.GetValue(window);
            var scaleField = areaObject.GetType().GetField("m_Scale", BindingFlags.Instance | BindingFlags.NonPublic);
            scaleField.SetValue(areaObject, new Vector2(desiredGameViewScale, desiredGameViewScale));
        }
#endif
    }
}