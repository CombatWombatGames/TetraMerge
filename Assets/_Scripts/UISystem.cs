using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Handles button clicks
public class UISystem : MonoBehaviour
{
    [Header("Top UI")]
    [SerializeField] Button menuButton = default;
    [SerializeField] Button helpButton = default;
    [SerializeField] Button undoButton = default;
    [SerializeField] Button closeHelpButton = default;
    [Header("Bottom UI")]
    [SerializeField] Button piecesButton = default;
    [SerializeField] Button boostersButton = default;
    [SerializeField] GameObject piecesPanel = default;
    [SerializeField] GameObject boostersPanel = default;
    [Header("Booster")]
    [SerializeField] Button refreshButton = default;
    [Header("Menu")]
    [SerializeField] Button continueButton = default;
    [SerializeField] Button restartButton = default;
    [SerializeField] Button collectionButton = default;
    [SerializeField] Button muteButton = default;
    [SerializeField] Button aboutButton = default;
    [SerializeField] Button quitButton = default;
    [SerializeField] GameObject menuCanvas = default;
    [SerializeField] GameObject aboutCanvas = default;
    [SerializeField] GameObject helpCanvas = default;
    [SerializeField] GameObject collectionCanvas = default;
    [SerializeField] Button closeMenuButton = default;
    [Header("About")]
    [SerializeField] Button mailButton = default;
    [SerializeField] Button termsButton = default;
    [SerializeField] Button privacyButton = default;
    [SerializeField] Button closeAboutButton = default;
    [Header("Collection")]
    [SerializeField] Rune runePrefab = default;
    [SerializeField] Button closeCollectionButton = default;
    [SerializeField] Transform runesParent = default;

    SaveSystem saveSystem;
    BoostersModel boosterModel;
    PlayerProgressionModel playerProgressionModel;
    Dictionary<string, GameObject> windows;

    void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
        boosterModel = GetComponent<BoostersModel>();
        playerProgressionModel = GetComponent<PlayerProgressionModel>();

        undoButton.onClick.AddListener(saveSystem.Undo);
        helpButton.onClick.AddListener(() => SetWindowActive(Consts.Help));
        menuButton.onClick.AddListener(() => SetWindowActive(Consts.Menu));
        closeHelpButton.onClick.AddListener(() => SetWindowActive(null));

        continueButton.onClick.AddListener(() => SetWindowActive(null));
        restartButton.onClick.AddListener(RestartScene);
        collectionButton.onClick.AddListener(() => SetWindowActive(Consts.Collection));
        muteButton.onClick.AddListener(Mute);
        aboutButton.onClick.AddListener(() => SetWindowActive(Consts.About));
        quitButton.onClick.AddListener(Quit);
        closeMenuButton.onClick.AddListener(() => SetWindowActive(null));

        piecesButton.onClick.AddListener(() => SwitchTable(true));
        boostersButton.onClick.AddListener(() => SwitchTable(false));
        refreshButton.onClick.AddListener(() => { boosterModel.GenerateNewPieces(); SwitchTable(true); });

        mailButton.onClick.AddListener(SendEmail);
        termsButton.onClick.AddListener(OpenTerms);
        privacyButton.onClick.AddListener(OpenPrivacy);
        closeAboutButton.onClick.AddListener(() => SetWindowActive(null));

        closeCollectionButton.onClick.AddListener(() => SetWindowActive(null));

        SwitchTable(true, false);

        windows = new Dictionary<string, GameObject> 
        { 
            { Consts.Menu, menuCanvas }, 
            { Consts.About, aboutCanvas }, 
            { Consts.Help, helpCanvas }, 
            { Consts.Collection, collectionCanvas } 
        };
    }

    void OnDestroy()
    {
        undoButton.onClick.RemoveAllListeners();
        helpButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        closeHelpButton.onClick.RemoveAllListeners();

        continueButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        collectionButton.onClick.RemoveAllListeners();
        muteButton.onClick.RemoveAllListeners();
        aboutButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        closeMenuButton.onClick.RemoveAllListeners();

        piecesButton.onClick.RemoveAllListeners();
        boostersButton.onClick.RemoveAllListeners();
        refreshButton.onClick.RemoveAllListeners();

        mailButton.onClick.RemoveAllListeners();
        termsButton.onClick.RemoveAllListeners();
        privacyButton.onClick.RemoveAllListeners();
        closeAboutButton.onClick.RemoveAllListeners();

        closeCollectionButton.onClick.RemoveAllListeners();
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
        }
        AudioSystem.Player.PlayButtonSfx();
        if (id == Consts.Collection)
        {
            SpawnRunes();
        }
    }

    public void RestartScene()
    {
        playerProgressionModel.UpdateBestScore();
        saveSystem.StartFromScratch();
        AudioSystem.Player.RestartMusic();
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

    void SpawnRunes()
    {
        Sprite[] tiles = GetComponent<Tiles>().TilesList;
        for (int i = 0; i < runesParent.childCount; i++)
        {
            Destroy(runesParent.GetChild(i).gameObject);
        }
        int unlockedCount = Mathf.Clamp(playerProgressionModel.BestLevel, 0, tiles.Length);
        for (int i = 0; i < unlockedCount; i++)
        {
            Rune rune = Instantiate(runePrefab, runesParent);
            rune.Initialize(tiles[i], Consts.runeDescriptions[i]);
        }
        if (unlockedCount < tiles.Length)
        {
            Instantiate(runePrefab, runesParent);
        }
    }
}