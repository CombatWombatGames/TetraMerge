using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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
    [SerializeField] Window menuCanvas = default;
    [SerializeField] Window aboutCanvas = default;
    [SerializeField] Window helpCanvas = default;
    [SerializeField] Window collectionCanvas = default;
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
    [Header("Help")]
    [SerializeField] ButtonEnhanced[] helpButtons = default;
    [SerializeField] VideoClip[] videos = default;
    [SerializeField] VideoPlayer videoPlayer = default;
    [SerializeField] Transform selection = default;

    SaveSystem saveSystem;
    BoostersModel boosterModel;
    PlayerProgressionModel playerProgressionModel;
    Dictionary<string, Window> windows;

    void Awake()
    {
        windows = new Dictionary<string, Window>
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
        helpButton.onClick.AddListener(() => OpenScroll(Consts.Help));
        helpButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        menuButton.onClick.AddListener(OpenMenu);
        menuButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayButtonSfx());
        closeHelpButton.onClick.AddListener(CloseScroll);
        //Bottom
        piecesButton.onPointerDown.AddListener(() => { if (piecesButton.interactable) SwitchTable(true); });
        boostersButton.onPointerDown.AddListener(() => { if (boostersButton.interactable) SwitchTable(false); });
        refreshButton.onClick.AddListener(() => { boosterModel.GenerateNewPieces(); SwitchTable(true); });
        ultimateButton.onClick.AddListener(() => { boosterModel.ClearBasicRunes(); SwitchTable(true); });
        ultimateButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayBoosterSfx());
        //Menu
        continueButton.onClick.AddListener(CloseMenu);
        continueButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayStoneSfx());
        restartButton.onClick.AddListener(RestartScene);
        restartButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayStoneSfx());
        collectionButton.onClick.AddListener(() => OpenScroll(Consts.Collection));
        collectionButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayStoneSfx());
        muteButton.onClick.AddListener(Mute);
        muteButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayStoneSfx());
        aboutButton.onClick.AddListener(() => OpenScroll(Consts.About));
        aboutButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayStoneSfx());
        quitButton.onClick.AddListener(Quit);
        quitButton.onPointerDown.AddListener(() => AudioSystem.Player.PlayStoneSfx());
        closeMenuButton.onClick.AddListener(CloseMenu);
        ravenButton.onPointerDown.AddListener(() => AnimationSystem.RavenBlink(ravenEye));
        //About
        mailButton.onClick.AddListener(SendEmail);
        termsButton.onClick.AddListener(OpenTerms);
        privacyButton.onClick.AddListener(OpenPrivacy);
        closeAboutButton.onClick.AddListener(CloseScroll);
        //Collection
        closeCollectionButton.onClick.AddListener(CloseScroll);

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

    void OpenMenu()
    {
        foreach (var kvp in windows)
        {
            kvp.Value.Canvas.SetActive(false);
        }
        AnimationSystem.OpenMenu(windows[Consts.Menu], ravenEye);
        AudioSystem.Player.PlayChainSfx();
        AnalyticsSystem.WindowOpen(Consts.Menu);
    }

    void OpenScroll(string id)
    {
        foreach (var kvp in windows)
        {
            kvp.Value.Canvas.SetActive(false);
        }
        AnimationSystem.OpenScroll(windows[id]);
        AudioSystem.Player.PlayPaperSlowSfx();
        AnalyticsSystem.WindowOpen(id);
        if (id == Consts.Collection)
        {
            InitializeRuneCollection();
        } 
        else if (id == Consts.Help)
        {
            InitializeHelp();
        }
    }

    void CloseMenu()
    {
        foreach (var kvp in windows)
        {
            kvp.Value.Canvas.SetActive(false);
        }
    }

    void CloseScroll()
    {
        foreach (var kvp in windows)
        {
            kvp.Value.Canvas.SetActive(false);
        }
        AudioSystem.Player.PlayPaperFastSfx();
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
            AudioSystem.Player.PlayWoodSfx();
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

    void InitializeHelp()
    {
        selection.position = helpButtons[0].transform.position;
        for (int i = 0; i < helpButtons.Length; i++)
        {
            helpButtons[i].onClick.RemoveAllListeners();
        }
        bool[] tutorialsAvailability = new bool[] 
        {
            true,
            boosterModel.BoostersOpen,
            playerProgressionModel.TurnNumber >= 20,
            playerProgressionModel.Stage > 0
        };
        int lastOpenTutorialIndex = 0;
        for (int i = 0; i < helpButtons.Length; i++)
        {
            InitializeButton(i, tutorialsAvailability[i]);
            if (tutorialsAvailability[i])
            {
                lastOpenTutorialIndex = i;
            }
        }
        SelectTutorial(lastOpenTutorialIndex);
    }

    void InitializeButton(int index, bool open)
    {
        helpButtons[index].GetComponent<GalleryButton>().Initialize(open, () => OnHelpButtonClicked(index, open));
    }

    void OnHelpButtonClicked(int index, bool open)
    {
        if (open)
        {
            AudioSystem.Player.PlayStoneSfx();
            SelectTutorial(index);
        }
        else
        {
            AnimationSystem.Rotate(helpButtons[index].GetComponentInChildren<Text>().transform);
        }
    }

    void SelectTutorial(int index)
    {
        AnimationSystem.MoveSelection(selection, helpButtons[index].transform.position);
        videoPlayer.Stop();
        videoPlayer.clip = videos[index];
        videoPlayer.Play();
    }

    void OnTurnChanged(int turn)
    {
        if (turn == 0 && playerProgressionModel.TotalMerged == 0)
        {
            OpenScroll(Consts.Help);
            GetComponent<InputDisabler>().DisableInput(2f);
        }
        //TODO LOW Merge once and restart will spawn buttons
        if (piecesButton.gameObject.activeSelf && !boosterModel.BoostersOpen)
        {
            piecesButton.gameObject.SetActive(false);
            boostersButton.gameObject.SetActive(false);
        }
        if (!piecesButton.gameObject.activeSelf && (boosterModel.BoostersOpen || boosterModel.AnyBoosterAvailable()))
        {
            AnimationSystem.ShowButtons(piecesButton, boostersButton);
            boosterModel.BoostersOpen = true;
        }
    }
}