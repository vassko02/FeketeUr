using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    // High Scores, Settings, és a Current Session Data
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
        if (currentRunData != null) // Ellenõrizd, hogy a CurrentRunData be van állítva
        {
            string json = JsonUtility.ToJson(currentRunData);

            PlayerPrefs.SetString("CurrentRun", json);
            PlayerPrefs.Save(); // Mentjük az adatokat
        }
        else
        {
            Debug.LogError("CurrentRunData is null! Cannot save.");
        }
    }
    public void LoadCurrentRun()
    {
        // Ellenõrizzük, hogy van-e mentett adat
        if (PlayerPrefs.HasKey("CurrentRun"))
        {
            string json = PlayerPrefs.GetString("CurrentRun");
            currentRunData = JsonUtility.FromJson<CurrentRun>(json);
        }
        else
        {
            // Ha nincs mentett adat, lehet új CurrentRun létrehozása itt
            currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
        }
    }
    public void SaveHighScores()
    {
        // Például a CurrentRun adatok mentése JSON formátumban
        string json = JsonUtility.ToJson(highScoresData);
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save(); // Mentjük az adatokat
    }
    public void SaveSettings()
    {
        // Például a CurrentRun adatok mentése JSON formátumban
        string json = JsonUtility.ToJson(settingsData);
        PlayerPrefs.SetString("Settings", json);
        PlayerPrefs.Save(); // Mentjük az adatokat
    }

}
