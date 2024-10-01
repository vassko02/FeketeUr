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
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            // Ha a mentett �rt�kek null�zva vannak, akkor letiltjuk a gombot
            if (loadedData==null)
            {
                continueButton.interactable = false; // Continue gomb letilt�sa
            }
        }
        else
        {
            // Ha nincs ment�s, akkor a Continue gombot szint�n letiltjuk
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
