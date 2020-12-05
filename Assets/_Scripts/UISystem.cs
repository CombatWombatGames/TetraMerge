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
    [SerializeField] Button muteButton = default;
    [SerializeField] Button aboutButton = default;
    [SerializeField] Button quitButton = default;
    [SerializeField] GameObject menuCanvas = default;
    [SerializeField] GameObject aboutCanvas = default;
    [SerializeField] GameObject helpCanvas = default;
    [Header("About")]
    [SerializeField] Button mailButton = default;
    [SerializeField] Button termsButton = default;
    [SerializeField] Button privacyButton = default;
    [SerializeField] Button closeAboutButton = default;

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
        muteButton.onClick.AddListener(Mute);
        aboutButton.onClick.AddListener(() => SetWindowActive(Consts.About));
        quitButton.onClick.AddListener(Quit);
        
        piecesButton.onClick.AddListener(() => SwitchTable(true));
        boostersButton.onClick.AddListener(() => SwitchTable(false));
        refreshButton.onClick.AddListener(() => { boosterModel.GenerateNewPieces(); SwitchTable(true); });

        mailButton.onClick.RemoveAllListeners();
        termsButton.onClick.RemoveAllListeners();
        privacyButton.onClick.RemoveAllListeners();
        closeAboutButton.onClick.AddListener(() => SetWindowActive(null));

        SwitchTable(true, false);

        windows = new Dictionary<string, GameObject> { { Consts.Menu, menuCanvas }, { Consts.About, aboutCanvas }, { Consts.Help, helpCanvas } };
    }

    void OnDestroy()
    {
        undoButton.onClick.RemoveAllListeners();
        helpButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        closeHelpButton.onClick.RemoveAllListeners();

        continueButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        muteButton.onClick.RemoveAllListeners();
        aboutButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();

        piecesButton.onClick.RemoveAllListeners();
        boostersButton.onClick.RemoveAllListeners();
        refreshButton.onClick.RemoveAllListeners();

        mailButton.onClick.RemoveAllListeners();
        termsButton.onClick.RemoveAllListeners();
        privacyButton.onClick.RemoveAllListeners();
        closeAboutButton.onClick.RemoveAllListeners();
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
}