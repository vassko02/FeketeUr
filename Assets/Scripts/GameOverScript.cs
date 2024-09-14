using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public void restart()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
