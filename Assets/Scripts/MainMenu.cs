using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class MainMenu : MonoBehaviour
{
    public Player player; // Referencia a játékosra


    // Ez a függvény hívódik, amikor az "Új játék" gombra kattintanak
    public void OnNewGameButtonClick()
    {
        // Átváltunk a Level1 scene-re

    }

    // Ez a függvény hívódik, amikor a "Continue" gombra kattintanak
    public void OnContinueButtonClick()
    {

        // Átváltunk a Level1 scene-re
        SceneManager.LoadScene("Level1");

        // Folytatjuk a mentett játékot
        StartCoroutine(WaitForSceneLoad(true));
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

        // Ha folytatjuk a játékot, akkor meghívjuk a LoadGame függvényt
        if (isContinuing)
        {
            player.newGame = false;

            player.LoadGame();
        }
        else
        {
            player.newGame = true;
            player.LoadGame(); // Vagy hagyhatod üresen, ha nem kell új játékhoz betölteni
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");

        // Új játékhoz nincs szükség mentés betöltésére
        StartCoroutine(WaitForSceneLoad(false));
    }
    public void Quit()
    {
        Application.Quit();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
