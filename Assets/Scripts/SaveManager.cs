using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    // High Scores, Settings, �s a Current Session Data
    public List<HighScore> highScoresData = new List<HighScore>();
    public Settings settingsData;
    public CurrentRun currentRunData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it between scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
    public void AddHighScore(HighScore newHS)
    {
        highScoresData.Add(newHS);
        highScoresData.Sort((a, b) => b.Score.CompareTo(a.Score)); // Descending order
        if (highScoresData.Count > 10) // Keep only top 10 scores
        {
            highScoresData.RemoveAt(highScoresData.Count - 1);
        }
    }
    public void UpdateCurrentRun(string playerName, int score, float elapsedTime, int maxHealth, int currentHealth, int scoreIncrement)
    {
        currentRunData = new CurrentRun(playerName, score, maxHealth, currentHealth, scoreIncrement, elapsedTime);
    }
    public void SaveCurrentRun()
    {
        if (currentRunData != null) // Ellen�rizd, hogy a CurrentRunData be van �ll�tva
        {
            string json = JsonUtility.ToJson(currentRunData);

            PlayerPrefs.SetString("CurrentRun", json);
            PlayerPrefs.Save(); // Mentj�k az adatokat
        }
        else
        {
            Debug.LogError("CurrentRunData is null! Cannot save.");
        }
    }
    public void LoadCurrentRun()
    {
        // Ellen�rizz�k, hogy van-e mentett adat
        if (PlayerPrefs.HasKey("CurrentRun"))
        {
            string json = PlayerPrefs.GetString("CurrentRun");
            currentRunData = JsonUtility.FromJson<CurrentRun>(json);
        }
        else
        {
            // Ha nincs mentett adat, lehet �j CurrentRun l�trehoz�sa itt
            currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
        }
    }
    public void SaveHighScores()
    {
        // P�ld�ul a CurrentRun adatok ment�se JSON form�tumban
        string json = JsonUtility.ToJson(highScoresData);
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save(); // Mentj�k az adatokat
    }
    public void SaveSettings()
    {
        // P�ld�ul a CurrentRun adatok ment�se JSON form�tumban
        string json = JsonUtility.ToJson(settingsData);
        PlayerPrefs.SetString("Settings", json);
        PlayerPrefs.Save(); // Mentj�k az adatokat
    }

}
