using UnityEngine;
using UnityEngine.UI;

// Handles button clicks
public class UISystem : MonoBehaviour
{
    [Header("Top UI")]
    [SerializeField] Button menuButton = default;
    [SerializeField] Button guiRestartButton = default;
    [SerializeField] Button undoButton = default;
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

    Menu menu;
    SaveSystem saveSystem;
    BoostersModel boosterModel;

    void Awake()
    {
        menu = GetComponent<Menu>();
        saveSystem = GetComponent<SaveSystem>();
        boosterModel = GetComponent<BoostersModel>();
        quitButton.onClick.AddListener(menu.Quit);
        restartButton.onClick.AddListener(menu.RestartScene);
        guiRestartButton.onClick.AddListener(menu.RestartScene);
        muteButton.onClick.AddListener(menu.Mute);
        undoButton.onClick.AddListener(saveSystem.Undo);
        piecesButton.onClick.AddListener(() => OnTableButtonClicked(true));
        boostersButton.onClick.AddListener(() => OnTableButtonClicked(false));
        refreshButton.onClick.AddListener(() => { boosterModel.GenerateNewPieces(); OnTableButtonClicked(true); });
        OnTableButtonClicked(true, false);
    }

    void OnDestroy()
    {
        quitButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        undoButton.onClick.RemoveAllListeners();
        piecesButton.onClick.RemoveAllListeners();
        boostersButton.onClick.RemoveAllListeners();
        muteButton.onClick.RemoveAllListeners();
    }

    void OnTableButtonClicked(bool enablePieces, bool sound = true)
    {
        piecesButton.interactable = !enablePieces;
        boostersButton.interactable = enablePieces;
        piecesPanel.SetActive(enablePieces);
        boostersPanel.SetActive(!enablePieces);
        piecesButton.transform.SetSiblingIndex(enablePieces? 2 : 0);
        boostersButton.transform.SetSiblingIndex(enablePieces ? 0 : 2);
        if (sound)
        {
            AudioSystem.Player.PlayButtonSfx();
        }
    }
}