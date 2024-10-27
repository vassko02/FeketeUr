using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
