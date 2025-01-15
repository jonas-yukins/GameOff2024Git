using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;
    string newGameScene = "MainScene";
    void Start()
    {
        SoundManager.Instance.MusicChannel.PlayOneShot(SoundManager.Instance.mainMenuMusic);
        // Set high score text
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Highscore: {highScore}";
    }

    public void StartNewGame()
    {
        SoundManager.Instance.MusicChannel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
        #if UNITY_EDITOR
            // if in editor, will exit play mode
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
