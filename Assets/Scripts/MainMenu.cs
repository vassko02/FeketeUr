using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Audio;
public class MainMenu : MonoBehaviour
{
    public Button continueButton; // Hozz�ad�s: Referencia a Continue gombhoz
    private string saveFilePath;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        Time.timeScale = 1f;
        SaveManager.Instance.Load();
        if (SaveManager.Instance.saveData.settingsData!=new Settings())
        {
            Resolution res = resolutions[SaveManager.Instance.saveData.settingsData.Resolution];
            Screen.SetResolution(res.width,res.height, SaveManager.Instance.saveData.settingsData.fullScreen);
        }
        if (SaveManager.Instance.saveData.currentRunData == null|| SaveManager.Instance.saveData.currentRunData.MaxHealth==0)
        {
            continueButton.interactable = false; // Continue gomb letilt�sa

        }
        else
        {
            continueButton.interactable = true;
        }
        MenuMusic backgroundMusic = FindObjectOfType<MenuMusic>();
        if (backgroundMusic != null)
        {
            backgroundMusic.GetComponent<AudioSource>().Play();
        }
    }
    
    public void PlayGame()
    {
        PlayerPrefs.SetInt("continue", 0);

        SceneManager.LoadScene("Intro");

    }
    public void OnContinueButtonClick()
    {
        PlayerPrefs.SetInt("continue",1);
        SceneManager.LoadScene("Level1");
    }
    public void LoadHighScores()
    {
        SceneManager.LoadScene("Leaderboard");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadSceneAsync("Options");
    }


}
