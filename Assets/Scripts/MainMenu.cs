using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class MainMenu : MonoBehaviour
{
    public Button continueButton; // Hozz�ad�s: Referencia a Continue gombhoz
    private string saveFilePath;
    void Start()
    {
        if (SaveManager.Instance.saveData.currentRunData == null|| SaveManager.Instance.saveData.currentRunData.MaxHealth==0)
        {
            continueButton.interactable = false; // Continue gomb letilt�sa

        }
        else
        {
            continueButton.interactable = true;

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
}
