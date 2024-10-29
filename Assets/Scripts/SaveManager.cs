using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveData saveData;
    // High Scores, Settings, és a Current Session Data
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
        saveData.highScoresData.Add(newHS);
        saveData.highScoresData.Sort((a, b) => b.Score.CompareTo(a.Score)); // Descending order
        if (saveData.highScoresData.Count > 10) // Keep only top 10 scores
        {
            saveData.highScoresData.RemoveAt(saveData.highScoresData.Count - 1);
        }
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(Instance.saveData, true);
        Debug.Log(json);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string json = PlayerPrefs.GetString("GameData");
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // Alapértelmezett értékeket állítunk be, ha nincs mentett adat
            saveData.currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
            saveData.settingsData = new Settings(); // vagy alapértelmezett értékek a beállításokhoz
        }
    }





    public void SaveCurrentRun()
    {
        if (saveData.currentRunData != null) // Ellenõrizd, hogy a CurrentRunData be van állítva
        {
            string json = JsonUtility.ToJson(saveData.currentRunData);

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
            saveData.currentRunData = JsonUtility.FromJson<CurrentRun>(json);
        }
        else
        {
            // Ha nincs mentett adat, lehet új CurrentRun létrehozása itt
            saveData.currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
        }
    }
    public void SaveHighScores()
    {
        // Például a CurrentRun adatok mentése JSON formátumban
        string json = JsonUtility.ToJson(saveData.highScoresData);
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save(); // Mentjük az adatokat
    }
    public void SaveSettings()
    {
        // Például a CurrentRun adatok mentése JSON formátumban
        string json = JsonUtility.ToJson(saveData.settingsData);
        PlayerPrefs.SetString("Settings", json);
        PlayerPrefs.Save(); // Mentjük az adatokat
    }

}
