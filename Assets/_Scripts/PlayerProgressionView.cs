using UnityEngine;
using UnityEngine.UI;

//Displays best and current scores
public class PlayerProgressionView : MonoBehaviour
{
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;
    [SerializeField] Text scoreText = default;
    [SerializeField] Text bestScoreText = default;

    void Awake()
    {
        playerProgressionModel.CurrentScoreChanged += OnCurrentScoreChanged;
        playerProgressionModel.BestScoreChanged += OnBestScoreChanged;
    }

    void OnDestroy()
    {
        playerProgressionModel.CurrentScoreChanged -= OnCurrentScoreChanged;
    }

    void Start()
    {
        bestScoreText.text = playerProgressionModel.BestScore.ToString();
    }

    void OnCurrentScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }

    void OnBestScoreChanged(int score)
    {
        bestScoreText.text = score.ToString();
    }
}
