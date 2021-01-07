using System.Collections.Generic;

public struct Consts
{
    public const string Menu = "Menu";
    public const string About = "About";
    public const string Help = "Help";
    public const string Collection = "Collection";

    public static Dictionary<MessageId, string> Messages = new Dictionary<MessageId, string> 
    {
        { MessageId.BoostersIncrement, "Random boosters earned!" },
        { MessageId.NewRune, "New rune added to collection!" },
        { MessageId.RuneUpdate, "Base rune updated!" },
        { MessageId.BonusFigureUnlock, "Bonus figure unlocked!" },
        { MessageId.BonusFigureChange, "Bonus figure changed!" },
        { MessageId.Victory1, "You have seen all the content we have prepared!" },
        { MessageId.Victory2, "So you won, congratulations!" },
    };

    public static string[] RuneDescriptions = new string[] 
    {
        "Tiwaz - the rune is named after one-handed god Týr.",//, and was identified with this god.",
        "Wynn - the denotation of the rune is \"joy\".",
        "Odal - its reconstructed name means \"heritage\".",
        "Fehu - its name means \"livestock\".",
        "Ur - the reconstructed name of the rune means \"water\".",
        "Peorð - the name could be referring to a fruit-tree.",
        "Gyfu - means \"gift\".",
        "Mannaz -  it is derived from the reconstructed word for \"man\".",
        "Algiz - from the word for \"moose\".",
        "Sowilō - means \"Sun\"."
    };

    public const string Text = "TWO FUP GMZS";
}

public enum MessageId
{
    BoostersIncrement,
    NewRune,
    RuneUpdate,
    BonusFigureUnlock,
    BonusFigureChange,
    Victory1,
    Victory2,
}