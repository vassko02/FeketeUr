using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{

    public void GoBack()
    {
        SceneManager.LoadSceneAsync("MainMenu");
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
