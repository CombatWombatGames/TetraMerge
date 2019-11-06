using UnityEngine;
using UnityEngine.UI;

//Displays best and current scores
public class PlayerProgressionView : MonoBehaviour
{
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    [SerializeField] Text turnNumberText = default;
    [SerializeField] Text scoreText = default;
    [SerializeField] Text bestScoreText = default;
    [SerializeField] Text levelNumberText = default;

    void Awake()
    {
        playerProgressionModel.TurnChanged += OnTurnChanged;
        playerProgressionModel.CurrentScoreChanged += OnCurrentScoreChanged;
        playerProgressionModel.BestScoreChanged += OnBestScoreChanged;
        playerProgressionModel.LevelNumberChanged += OnLevelNumberChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.TurnChanged -= OnTurnChanged;
        playerProgressionModel.CurrentScoreChanged -= OnCurrentScoreChanged;
        playerProgressionModel.BestScoreChanged -= OnBestScoreChanged;
        playerProgressionModel.LevelNumberChanged -= OnLevelNumberChanged;
    }

    void Start()
    {
        bestScoreText.text = playerProgressionModel.BestScore.ToString();
    }

    void OnTurnChanged(int turnNumber)
    {
        turnNumberText.text = turnNumber.ToString();
    }

    void OnCurrentScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }

    void OnBestScoreChanged(int score)
    {
        bestScoreText.text = score.ToString();
    }

    void OnLevelNumberChanged(int score)
    {
        levelNumberText.text = score.ToString();
    }
}
