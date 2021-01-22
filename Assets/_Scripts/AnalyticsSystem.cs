using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsSystem
{
    static void LevelStart(int level)
    {
        //Add boosters
        //Add score
        //Add stage
        AnalyticsEvent.LevelStart(level);
    }

    static void StageStart(int stage)
    {
        //Add boosters
        //Add score
        //Add level
        AnalyticsEvent.LevelUp(stage);
    }

    static void Restart(int level)
    {
        //Add boosters
        //Add score
        //Add stage
        AnalyticsEvent.LevelFail(level);
    }

    static void WindowOpen(string name)
    {
        AnalyticsEvent.ScreenVisit(name);
    }

    static void BoosterAcquired(string name)
    {
        //Add level
        //Add stage
        AnalyticsEvent.Custom("BoosterAcquired", new Dictionary<string, object> { { "Name", name } });
    }

    static void BoosterUsed(string name)
    {
        //Add level
        //Add stage
        AnalyticsEvent.Custom("BoosterUsed", new Dictionary<string, object> { { "Name", name } });
    }

    static void Merge(int diagonal)
    {
        //Add level
        //Add stage
        AnalyticsEvent.Custom("Merge", new Dictionary<string, object> { { "Name", diagonal } });
    }
}
