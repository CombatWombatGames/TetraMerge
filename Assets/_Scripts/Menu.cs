using UnityEditor;
using UnityEngine;

//For dirty shortcuts during development
public class Menu : MonoBehaviour
{
    public void RestartScene()
    {
        GetComponent<PlayerProgressionModel>().UpdateBestScore();
        GetComponent<SaveSystem>().StartFromScratch();
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
}