using System.Collections.Generic;

public struct Consts
{
    public const string Menu = "Menu";
    public const string About = "About";
    public const string Help = "Help";
    public const string Collection = "Collection";

    public static Dictionary<MessageId, string> Messages = new Dictionary<MessageId, string> 
    {
        { MessageId.BoostersIncremented, "Random booster earned!" },
        { MessageId.NewRuneCollected, "New rune added to collection!" },
        { MessageId.BaseRuneUpdated, "Base rune updated!" },
        { MessageId.StageChanged, "New stage!" },
        { MessageId.BonusFigureUnlocked, "Bonus figure unlocked!" },
        { MessageId.BonusFigureChanged, "Bonus figure changed!" },
        { MessageId.Victory1, "You have beat all the content we have prepared!" },
        { MessageId.Victory2, "So you won, congratulations!" },
    };

    public static string[] RuneDescriptions = new string[] 
    {
        "Tiwaz - the rune is named after one-handed god Tyr.",//Týr
        "Wynn - the denotation of the rune is \"joy\".",
        "Odal - its reconstructed name means \"heritage\".",
        "Fehu - its name means \"livestock\".",
        "Ur - the reconstructed name of the rune means \"water\".",
        "Peorth - the name could be referring to a fruit-tree.",//Peorð
        "Gyfu - means \"gift\".",
        "Mannaz -  it is derived from the reconstructed word for \"man\".",
        "Algiz - from the word for \"moose\".",
        "Sowilo - means \"Sun\"."//Sowilō
    };

    public const string Text = "TWO FUP GMZS";
}

public enum MessageId
{
    BoostersIncremented,
    NewRuneCollected,
    BaseRuneUpdated,
    StageChanged,
    BonusFigureUnlocked,
    BonusFigureChanged,
    Victory1,
    Victory2,
}