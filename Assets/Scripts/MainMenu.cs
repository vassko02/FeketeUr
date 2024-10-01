using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class MainMenu : MonoBehaviour
{
    public Player player; // Referencia a j�t�kosra


    // Ez a f�ggv�ny h�v�dik, amikor az "�j j�t�k" gombra kattintanak
    public void OnNewGameButtonClick()
    {
        // �tv�ltunk a Level1 scene-re

    }

    // Ez a f�ggv�ny h�v�dik, amikor a "Continue" gombra kattintanak
    public void OnContinueButtonClick()
    {

        // �tv�ltunk a Level1 scene-re
        SceneManager.LoadScene("Level1");

        // Folytatjuk a mentett j�t�kot
        StartCoroutine(WaitForSceneLoad(true));
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

        // Ha folytatjuk a j�t�kot, akkor megh�vjuk a LoadGame f�ggv�nyt
        if (isContinuing)
        {
            player.newGame = false;

            player.LoadGame();
        }
        else
        {
            player.newGame = true;
            player.LoadGame(); // Vagy hagyhatod �resen, ha nem kell �j j�t�khoz bet�lteni
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");

        // �j j�t�khoz nincs sz�ks�g ment�s bet�lt�s�re
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
