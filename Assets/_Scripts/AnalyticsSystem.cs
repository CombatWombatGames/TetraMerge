using System.Collections.Generic;
using UnityEngine.Analytics;

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
        AnalyticsEvent.Custom("booster_acquired", new Dictionary<string, object> (parameters) { { "name", type } });
    }

    public static void BoosterUsed(BoosterType type)
    {
        AnalyticsEvent.Custom("booster_used", new Dictionary<string, object>(parameters) { { "name", type } });
    }

    public static void Merge(int count)
    {
        AnalyticsEvent.Custom("merge", new Dictionary<string, object>(parameters) { { "count", count } });
    }
}
