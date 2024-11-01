using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
