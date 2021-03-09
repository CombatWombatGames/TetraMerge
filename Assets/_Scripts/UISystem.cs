using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Handles button clicks
public class UISystem : MonoBehaviour
{
    [Header("Top UI")]
    [SerializeField] ButtonEnhanced menuButton = default;
    [SerializeField] ButtonEnhanced helpButton = default;
    [SerializeField] ButtonEnhanced undoButton = default;
    [SerializeField] Button closeHelpButton = default;
    [Header("Bottom UI")]
    [SerializeField] ButtonEnhanced piecesButton = default;
    [SerializeField] ButtonEnhanced boostersButton = default;
    [SerializeField] GameObject piecesPanel = default;
    [SerializeField] GameObject boostersPanel = default;
    [SerializeField] ButtonEnhanced refreshButton = default;
    [SerializeField] ButtonEnhanced ultimateButton = default;
    [Header("Menu")]
    [SerializeField] ButtonEnhanced continueButton = default;
    [SerializeField] ButtonEnhanced restartButton = default;
    [SerializeField] ButtonEnhanced collectionButton = default;
    [SerializeField] ButtonEnhanced muteButton = default;
    [SerializeField] ButtonEnhanced aboutButton = default;
    [SerializeField] ButtonEnhanced quitButton = default;
    [SerializeField] ButtonEnhanced ravenButton = default;
    [SerializeField] Button closeMenuButton = default;
    [SerializeField] GameObject menuCanvas = default;
    [SerializeField] GameObject aboutCanvas = default;
    [SerializeField] GameObject helpCanvas = default;
    [SerializeField] GameObject collectionCanvas = default;
    [SerializeField] Image menuBackground = default;
    [SerializeField] Transform menuPanel = default;
    [SerializeField] Transform ravenEye = default;
    [Header("About")]
    [SerializeField] Button mailButton = default;
    [SerializeField] Button termsButton = default;
    [SerializeField] Button privacyButton = default;
    [SerializeField] Button closeAboutButton = default;
    [Header("Collection")]
    [SerializeField] Rune runePrefab = default;
    [SerializeField] Button closeCollectionButton = default;
    [SerializeField] Transform runesParent = default;
    [SerializeField] Text collectedRunes = default;

    SaveSystem saveSystem;
    BoostersModel boosterModel;
    PlayerProgressionModel playerProgressionModel;
    Dictionary<string, GameObject> windows;

    void Awake()
    {
        windows = new Dictionary<string, GameObject>
        {
            { Consts.Menu, menuCanvas },
            { Consts.About, aboutCanvas },
            { Consts.Help, helpCanvas },
            { Consts.Collection, collectionCanvas }
        };
        saveSystem = GetComponent<SaveSystem>();
        boosterModel = GetComponent<BoostersModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        //Top
        undoButton.onClick.AddListener(saveSystem.Undo);
        undoButton.onPointerDown.AddListener(() => { if (undoButton.interactable) AudioSystem.Player.PlayButtonSfx(); });
        helpButton.onClick.AddListener(() => SetWindowActive(Consts.Help));
        helpButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        menuButton.onClick.AddListener(OpenMenu);
        menuButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        closeHelpButton.onClick.AddListener(() => SetWindowActive(null));
        //Bottom
        piecesButton.onPointerDown.AddListener(() => { if (piecesButton.interactable) SwitchTable(true); });
        boostersButton.onPointerDown.AddListener(() => { if (boostersButton.interactable) SwitchTable(false); });
        refreshButton.onClick.AddListener(() => { boosterModel.GenerateNewPieces(); SwitchTable(true); });
        ultimateButton.onClick.AddListener(() => { boosterModel.ClearBasicRunes(); SwitchTable(true); });
        ultimateButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayBoosterSfx());
        //Menu
        continueButton.onClick.AddListener(() => SetWindowActive(null));
        continueButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        restartButton.onClick.AddListener(RestartScene);
        restartButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        collectionButton.onClick.AddListener(() => SetWindowActive(Consts.Collection));
        collectionButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        muteButton.onClick.AddListener(Mute);
        muteButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        aboutButton.onClick.AddListener(() => SetWindowActive(Consts.About));
        aboutButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        quitButton.onClick.AddListener(Quit);
        quitButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        closeMenuButton.onClick.AddListener(() => SetWindowActive(null));
        ravenButton.onPointerDown.AddListener(() => AnimationSystem.RavenBlink(ravenEye));
        //About
        mailButton.onClick.AddListener(SendEmail);
        termsButton.onClick.AddListener(OpenTerms);
        privacyButton.onClick.AddListener(OpenPrivacy);
        closeAboutButton.onClick.AddListener(() => SetWindowActive(null));
        //Collection
        closeCollectionButton.onClick.AddListener(() => SetWindowActive(null));

        SwitchTable(true, false);
        playerProgressionModel.TurnChanged += OnTurnChanged;
    }

    void OnDestroy()
    {
        //ButtonEnhanced removes all listeners on destroy by itself
        //Top
        closeHelpButton.onClick.RemoveAllListeners();
        //Menu
        closeMenuButton.onClick.RemoveAllListeners();
        //About
        mailButton.onClick.RemoveAllListeners();
        termsButton.onClick.RemoveAllListeners();
        privacyButton.onClick.RemoveAllListeners();
        closeAboutButton.onClick.RemoveAllListeners();
        //Collection
        closeCollectionButton.onClick.RemoveAllListeners();

        playerProgressionModel.TurnChanged -= OnTurnChanged;
    }

    void SetWindowActive(string id)
    {
        foreach (var kvp in windows)
        {
            kvp.Value.SetActive(false);
        }
        if (!string.IsNullOrEmpty(id))
        {
            windows[id].SetActive(true);
            AnalyticsSystem.WindowOpen(id);
        }
        if (id == Consts.Collection)
        {
            InitializeRuneCollection();
        }
        if (id == Consts.Help || id == Consts.Collection || id == Consts.About)
        {
            AudioSystem.Player.PlayPaperSlowSfx();
        }
    }

    void OpenMenu()
    {
        foreach (var kvp in windows)
        {
            kvp.Value.SetActive(false);
        }
        AnimationSystem.OpenMenu(windows[Consts.Menu], menuBackground, menuPanel, ravenEye);
        AudioSystem.Player.PlayChainSfx();
    }

    public void RestartScene()
    {
        playerProgressionModel.UpdateBestScore();
        saveSystem.StartFromScratch();
        AudioSystem.Player.RestartMusicWithFading();
    }

    public void Mute()
    {
        AudioSystem.Player.MuteMusic();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SwitchTable(bool enablePieces, bool sound = true)
    {
        piecesButton.interactable = !enablePieces;
        boostersButton.interactable = enablePieces;
        piecesPanel.SetActive(enablePieces);
        boostersPanel.SetActive(!enablePieces);
        piecesButton.transform.SetSiblingIndex(enablePieces ? 2 : 0);
        boostersButton.transform.SetSiblingIndex(enablePieces ? 0 : 2);
        if (sound)
        {
            AudioSystem.Player.PlayButtonSfx();
        }
    }

    void SendEmail()
    {
        Application.OpenURL("mailto:combatwombat@ro.ru");
    }

    void OpenTerms()
    {
        Application.OpenURL("https://docs.google.com/document/d/1eige_tr6YDSQ8-XE1gdLG8ZbNeGmVvD4Mex3bHjJYxA/edit?usp=sharing");
    }

    void OpenPrivacy()
    {
        Application.OpenURL("https://docs.google.com/document/d/1gVyA0oGOmKJ9KjuA-RmlUS0nkkNwe3kmVs1v3bX29FA/edit?usp=sharing");
    }

    void InitializeRuneCollection()
    {
        Sprite[] tiles = GetComponent<Resources>().TilesList;
        for (int i = 1; i < runesParent.childCount; i++)
        {
            Destroy(runesParent.GetChild(i).gameObject);
        }
        int unlockedCount = Mathf.Clamp(playerProgressionModel.BestRune, 0, tiles.Length);
        for (int i = 0; i < unlockedCount; i++)
        {
            Rune rune = Instantiate(runePrefab, runesParent);
            rune.Initialize(tiles[i], Consts.RuneDescriptions[i]);
        }
        if (unlockedCount < tiles.Length)
        {
            Instantiate(runePrefab, runesParent);
        }
        collectedRunes.text = $"You have collected {playerProgressionModel.TotalMerged} runes";
    }

    void OnTurnChanged(int turn)
    {
        if (turn == 3 && playerProgressionModel.TotalMerged == 0)
        {
            SetWindowActive(Consts.Help);
        }
        if (boosterModel.BoostersGiven == 0)
        {
            piecesButton.gameObject.SetActive(false);
            boostersButton.gameObject.SetActive(false);
        }
        else if (boosterModel.BoostersGiven == 1)
        {
            piecesButton.gameObject.SetActive(true);
            boostersButton.gameObject.SetActive(true);
        }
    }
}