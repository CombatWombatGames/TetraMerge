using UnityEngine;
using UnityEngine.UI;

public class LevelsProgressionView : MonoBehaviour
{
    [SerializeField] GameObject levelButtonsParent = default;

    LevelsProgression levelsProgression;
    Button[] levelsButtons;

    void Awake()
    {
        InitialiseEvents();
        GetButtons();
        UpdateView(levelsProgression.MaximumLevel);
    }

    void InitialiseEvents()
    {
        levelsProgression = gameObject.GetComponent<LevelsProgression>();
        levelsProgression.CurrentLevelChanged += OnCurrentLevelChanged;
        levelsProgression.MaximumLevelChanged += OnMaximumLevelChanged;
    }

    void GetButtons()
    {
        levelsButtons = levelButtonsParent.GetComponentsInChildren<Button>();
    }

    void OnCurrentLevelChanged(int currentLevel) { }

    void OnMaximumLevelChanged(int maximumLevel)
    {
        UpdateView(maximumLevel);
    }

    void UpdateView(int maximumLevel)
    {
        for (int i = 0; i < maximumLevel; i++)
        {
            levelsButtons[i].interactable = true;
        }
    }

    void OnDisable()
    {
        levelsProgression.CurrentLevelChanged -= OnCurrentLevelChanged;
        levelsProgression.MaximumLevelChanged -= OnMaximumLevelChanged;
    }
}
