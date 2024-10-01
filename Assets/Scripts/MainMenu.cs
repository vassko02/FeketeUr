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
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            // Ha a mentett értékek nullázva vannak, akkor letiltjuk a gombot
            if (loadedData==null)
            {
                continueButton.interactable = false; // Continue gomb letiltása
            }
        }
        else
        {
            // Ha nincs mentés, akkor a Continue gombot szintén letiltjuk
            continueButton.interactable = false;
        }
    }


    // Coroutine, amely várja a scene betöltését, és keresi a játékost
    private IEnumerator WaitForSceneLoad(bool isContinuing)
    {
        // Várakozás a scene betöltésére
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level1");

        // Miután betöltõdött a scene, keressük meg a játékost
        Player player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("Nem található a Player a Level1 scene-ben!");
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
