using UnityEngine;
using UnityEngine.UI;

//Displays best and current scores
public class PlayerProgressionView : MonoBehaviour
{
    [SerializeField] Text turnNumberText = default;
    [SerializeField] Text scoreText = default;
    [SerializeField] Text bestScoreText = default;
    [SerializeField] Text levelNumberText = default;
    [SerializeField] Text cupScoreText = default;
    [SerializeField] Image cup = default;
    [SerializeField] GameObject scores = default;

    PlayerProgressionModel playerProgressionModel;
    Resources resources;

    void Awake()
    {
        playerProgressionModel = GetComponent<PlayerProgressionModel>();
        resources = GetComponent<Resources>();
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
        if (playerProgressionModel.StagesComplete)
        {
            cup.sprite = resources.CupsList[2];
            cupScoreText.text = "";
            scores.SetActive(true);
        }
        else if(playerProgressionModel.BestRune < resources.TilesList.Length)
        {
            cup.sprite = resources.CupsList[0];
            cupScoreText.text = $"{playerProgressionModel.BestRune}/{resources.TilesList.Length}";
            scores.SetActive(false);
        }
        else if (playerProgressionModel.Stage < resources.StagesList.Length)
        {
            cup.sprite = resources.CupsList[1];
            cupScoreText.text = $"{playerProgressionModel.Stage}/{resources.StagesList.Length}";
            scores.SetActive(false);
        }
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
