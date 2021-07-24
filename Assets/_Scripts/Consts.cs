using System.Collections.Generic;
using UnityEngine;

public struct Consts
{
    public const string Menu = "Menu";
    public const string About = "About";
    public const string Help = "Help";
    public const string Collection = "Collection";

    public static Dictionary<MessageId, string> Messages => Application.systemLanguage == SystemLanguage.Russian ? messagesRu : messagesEn;

    static Dictionary<MessageId, string> messagesEn = new Dictionary<MessageId, string>
    {
        { MessageId.BoostersIncremented, "Random boosters earned!" },
        { MessageId.NewRuneCollected, "New rune added to collection!" },
        { MessageId.BaseRuneUpdated, "Base rune updated!" },
        { MessageId.StageChanged, "New stage!" },
        { MessageId.BonusFigureUnlocked, "Bonus figure unlocked!" },
        { MessageId.BonusFigureChanged, "Bonus figure changed!" },
        { MessageId.Victory1, "You have beat all the content we have prepared!" },
        { MessageId.Victory2, "So you won, congratulations!" },
    };

    static Dictionary<MessageId, string> messagesRu = new Dictionary<MessageId, string> 
    {
        { MessageId.BoostersIncremented, "Бустеры получены!" },
        { MessageId.NewRuneCollected, "Новая руна в коллекции!" },
        { MessageId.BaseRuneUpdated, "Базовая руна усилена!" },
        { MessageId.StageChanged, "Новый этап!" },
        { MessageId.BonusFigureUnlocked, "Открыта бонусная фигура!" },
        { MessageId.BonusFigureChanged, "Обновлена бонусная фигура!" },
        { MessageId.Victory1, "Вы прошли весь контент в игре!" },
        { MessageId.Victory2, "Это победа, поздравляю!" },
    };

    public static string[] RuneDescriptions => Application.systemLanguage == SystemLanguage.Russian ? runeDescriptionsRu : runeDescriptionsEn;

    static string[] runeDescriptionsEn = new string[] 
    {
        "Tiwaz - god Tyr.",//Týr
        "Wynn - \"joy\".",
        "Odal - \"heritage\".",
        "Fehu - \"livestock\".",
        "Ur - \"water\".",
        "Peorth - \"fruit-tree\".",//Peorð
        "Gyfu - \"gift\".",
        "Mannaz -  \"man\".",
        "Algiz - \"moose\".",
        "Sowilo - \"Sun\"."//Sowilō
    };

    static string[] runeDescriptionsRu = new string[]
    {
        "Tiwaz - бог Тюр.",
        "Wynn - \"радость\".",
        "Odal - \"наследие\".",
        "Fehu - \"скот\".",
        "Ur - \"вода\".",
        "Peorth - \"дерево\".",
        "Gyfu - \"подарок\".",
        "Mannaz -  \"человек\".",
        "Algiz - \"лось\".",
        "Sowilo - \"Солнце\"."
    };

    public static string[] Tutorials => Application.systemLanguage == SystemLanguage.Russian ? tutorialsRu : tutorialsEn;

    static string[] tutorialsEn = new string[]
    {
        "1. Tap a piece to rotate.\n2. Drag pieces to the field.\n3. Select square area to merge.<color=yellow>\n4. Get rid of all basic runes!</color>",
        "1. You can undo your last turn.\n2. Level up pieces by removing all basic runes!",
        "Boosters appear once in a while: pink bar, merge > 4x4, basic rune level up.\n\"<color=yellow>Refresh</color>\" booster respawns pieces.\n\"<color=green>Add</color>\" booster creates a rune.\n\"<color=red>Remove</color>\" booster destroys a rune.",
        "Merge entire field to get to the next stage with new obstacles!",
        "Ultimate booster destroys all basic runes and boosters, but available only once per stage!",
    };

    static string[] tutorialsRu = new string[]
    {
        "1. Нажмите на фигуру, чтобы повернуть.\n2. Перетащите фигуру на поле.\n3. Выделите квадратную область, чтобы её объединить и поднять уровень.<color=yellow>\n4. Избавьтесь от всех базовых рун!</color>",
        "Последний ход можно отменить.\nПоднимите уровень фигур снизу, убрав все базовые руны с поля!",
        "Бустеры добываются разными путями: розовая шкала, сворачивание > 4x4, подъём уровня фигур.\nБустер \"<color=yellow>Refresh</color>\" обновляет фигуры.\nБустер \"<color=green>Add</color>\" создаёт руну.\nБустер \"<color=red>Remove</color>\" удаляет руну.",
        "Сверните всё поле, чтобы перейти на следующий этап с новыми препятствиями!",
        "Финальный бустер уничтожает все базовые руны и бустеры, но только один раз за этап!",
    };
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