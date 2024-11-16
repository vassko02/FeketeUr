using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public ProgressManager progressManager;
    public Player player;
    private SaveManager saveManager=>SaveManager.Instance;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.pause = false; // Hangok újraindítása

    }
    void Pause()
    {
        SaveManager.Instance.saveData.currentRunData.CurrentHealth = player.currentHealth;
        SaveManager.Instance.saveData.currentRunData.MaxHealth = player.maxHealt;
        SaveManager.Instance.saveData.currentRunData.ScoreIncrement = player.scoreIncrement;
        SaveManager.Instance.saveData.currentRunData.Score = player.score;
        SaveManager.Instance.saveData.currentRunData.ElapsedTime = progressManager.elapsedTime;
        SaveManager.Instance.saveData.currentRunData.PlayerName = player.playerName;
        saveManager.Save();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        AudioListener.pause = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Resume();
    }
}
