using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressionView : MonoBehaviour
{
    [SerializeField] PlayerProgression playerProgression = default;
    [SerializeField] Text scoreText = default;

    void Awake()
    {
        playerProgression.CurrentScoreChanged += OnCurrentScoreChanged;
    }

    void OnDestroy()
    {
        playerProgression.CurrentScoreChanged -= OnCurrentScoreChanged;
    }

    void OnCurrentScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }
}
