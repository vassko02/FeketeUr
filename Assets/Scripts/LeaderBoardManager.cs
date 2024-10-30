using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoardManager : MonoBehaviour
{
    public SaveManager SaveManager => SaveManager.Instance;
    public GameObject row;
    private void Start()
    {

        var sortedList = SaveManager.saveData.highScoresData;
        sortedList.Sort((a, b) => a.Score.CompareTo(b.Score));
        for (int i = 0; i < sortedList.Count; i++)
        {
            var obj = Instantiate(row, row.transform.parent);
            obj.SetActive(true);
            obj.GetComponent<LeaderBoardItem>().Init((i + 1) + ".", sortedList[i].PlayerName, sortedList[i].Score.ToString());
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
