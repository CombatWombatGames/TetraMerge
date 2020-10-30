using UnityEngine;
using UnityEngine.UI;

// Handles button clicks
public class UISystem : MonoBehaviour
{
    [SerializeField] Button quitButton = default;
    [SerializeField] Button restartButton = default;
    [SerializeField] Button undoButton = default;
    [SerializeField] Button piecesButton = default;
    [SerializeField] Button boostersButton = default;
    [SerializeField] GameObject piecesPanel = default;
    [SerializeField] GameObject boostersPanel = default;
    [SerializeField] Button refreshButton = default;

    Tester tester;
    SaveSystem saveSystem;
    BoostersModel boosterModel;

    void Awake()
    {
        tester = GetComponent<Tester>();
        saveSystem = GetComponent<SaveSystem>();
        boosterModel = GetComponent<BoostersModel>();
        quitButton.onClick.AddListener(tester.Quit);
        restartButton.onClick.AddListener(tester.RestartScene);
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
            FindObjectOfType<AudioSystem>().PlayButtonSfx();
        }
    }
}