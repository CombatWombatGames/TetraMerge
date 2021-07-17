using System.Collections.Generic;

public class AnalyticsSystem
{
    static PlayerProgressionModel playerProgressionModel;

    static Dictionary<string, object> parameters { get { return new Dictionary<string, object> {
            { "turn", playerProgressionModel.TurnNumber },
            { "level", playerProgressionModel.LevelNumber },
            { "stage", playerProgressionModel.Stage },
            { "score", playerProgressionModel.CurrentScore },
    };}}

    public static void Initialize(PlayerProgressionModel playerProgressionModel)
    {
        AnalyticsSystem.playerProgressionModel = playerProgressionModel;
#if false
        if (UnityEngine.PlayerPrefs.GetInt("FirstLaunch", 0) == 0)
        {
            FreeVersionFirstLaunch();
            UnityEngine.PlayerPrefs.SetInt("FirstLaunch", 1);
        }
#endif
    }

    public static void LevelStart()
    {
        SendEvent("level_start", parameters);
    }

    public static void StageStart()
    {
        SendEvent("stage_start", parameters);
    }

    public static void Restart()
    {
        SendEvent("restart", parameters);
    }

    public static void WindowOpen(string name)
    {
        SendEvent("window_open", new Dictionary<string, object>(parameters) { { "name", name } });
    }

    public static void BoosterAcquired(BoosterType type)
    {
        SendEvent("booster_acquired", new Dictionary<string, object> (parameters) { { "name", type } });
    }

    public static void BoosterUsed(BoosterType type)
    {
        SendEvent("booster_used", new Dictionary<string, object>(parameters) { { "name", type } });
    }

    public static void Merge(int area)
    {
        SendEvent("merge", new Dictionary<string, object>(parameters) { { "area", area } });
    }

    public static void FreeVersionFirstLaunch()
    {
        SendEvent("free_version_first_launch");
    }

    public static void SendEvent(string name, Dictionary<string, object> data = null)
    {
#if UNITY_EDITOR
        string message = $"Event \"{name}\" fired";
        if (data != null)
        {
            foreach (var parameter in data)
            {
                message += $", {parameter.Key}: {parameter.Value}";
            }
        }
        UnityEngine.Debug.Log(message);
#else
        if (data == null)
        {
            UnityEngine.Analytics.AnalyticsEvent.Custom(name);
        }
        else
        {
            UnityEngine.Analytics.AnalyticsEvent.Custom(name, data);
        }
#endif
    }
}
