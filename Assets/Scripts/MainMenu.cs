using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class MainMenu : MonoBehaviour
{
    public Button continueButton; // Hozzáadás: Referencia a Continue gombhoz
    private string saveFilePath;
    void Start()
    {
        if (PlayerPrefs.GetInt("isAlive")==0)
        {
            continueButton.interactable = false; // Continue gomb letiltása

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
    public void Quit()
    {
        Application.Quit();
    }
}
