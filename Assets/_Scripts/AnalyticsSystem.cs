using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsSystem
{
    static PlayerProgressionModel playerProgressionModel;

    static Dictionary<string, object> parameters { get { return new Dictionary<string, object> {
            { "Turn", playerProgressionModel.TurnNumber },
            { "Level", playerProgressionModel.LevelNumber },
            { "Stage", playerProgressionModel.Stage },
            { "Score", playerProgressionModel.CurrentScore },
    };}}

    public static void Initialize(PlayerProgressionModel playerProgressionModel)
    {
        AnalyticsSystem.playerProgressionModel = playerProgressionModel;
    }

    public static void LevelStart(int level)
    {
        AnalyticsEvent.LevelStart(level, parameters);
    }

    public static void StageStart(int stage)
    {
        AnalyticsEvent.LevelUp(stage, parameters);
    }

    public static void Restart(int level)
    {
        AnalyticsEvent.LevelFail(level, parameters);
    }

    public static void WindowOpen(string name)
    {
        AnalyticsEvent.ScreenVisit(name);
    }

    public static void BoosterAcquired(BoosterType type)
    {
        AnalyticsEvent.Custom("BoosterAcquired", new Dictionary<string, object> (parameters) { { "Name", type } });
    }

    public static void BoosterUsed(BoosterType type)
    {
        AnalyticsEvent.Custom("BoosterUsed", new Dictionary<string, object>(parameters) { { "Name", type } });
    }

    public static void Merge(int count)
    {
        AnalyticsEvent.Custom("Merge", new Dictionary<string, object>(parameters) { { "Count", count } });
    }
}
