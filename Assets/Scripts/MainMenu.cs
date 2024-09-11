using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("klikk");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Level1");
    }
    public void Quit()
    {
        Debug.Log("asd");    
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
