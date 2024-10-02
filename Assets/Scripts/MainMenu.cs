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
        if (PlayerPrefs.GetInt("score", 0)==0)
        {
            continueButton.interactable = false; // Continue gomb letilt�sa

        }
        else
        {
            continueButton.interactable = false;

        }
    }


    // Coroutine, amely v�rja a scene bet�lt�s�t, �s keresi a j�t�kost
    private IEnumerator WaitForSceneLoad(bool isContinuing)
    {
        // V�rakoz�s a scene bet�lt�s�re
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level1");

        // Miut�n bet�lt�d�tt a scene, keress�k meg a j�t�kost
        Player player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("Nem tal�lhat� a Player a Level1 scene-ben!");
            yield break;
        }

        if (isContinuing)
        {
            player.LoadGame();

        }
        else
        {
            player.InitializePlayer(true);
        }
    }
    public void PlayGame()
    { 
        SceneManager.LoadScene("Level1");

        StartCoroutine(WaitForSceneLoad(false));
    }
    public void OnContinueButtonClick()
    {
        SceneManager.LoadScene("Level1");

        StartCoroutine(WaitForSceneLoad(true));
    }
    public void Quit()
    {
        Application.Quit();
    }
}
