using UnityEngine;

//Provides methods for progression-related buttons
public class PlayerProgressionController : MonoBehaviour
{
    [SerializeField] PlayerProgressionModel playerProgressionModel = default;

    //UGUI
    public void ClaimBest()
    {
        int bestScore = playerProgressionModel.BestScore;
        int currentScore = playerProgressionModel.CurrentScore;
        if (currentScore > bestScore)
        {
            playerProgressionModel.BestScore = currentScore;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}