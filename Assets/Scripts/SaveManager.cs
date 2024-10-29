using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveData saveData;
    // High Scores, Settings, �s a Current Session Data
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
            // Alap�rtelmezett �rt�keket �ll�tunk be, ha nincs mentett adat
            saveData.currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
            saveData.settingsData = new Settings(); // vagy alap�rtelmezett �rt�kek a be�ll�t�sokhoz
        }
    }





    public void SaveCurrentRun()
    {
        if (saveData.currentRunData != null) // Ellen�rizd, hogy a CurrentRunData be van �ll�tva
        {
            string json = JsonUtility.ToJson(saveData.currentRunData);

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
            saveData.currentRunData = JsonUtility.FromJson<CurrentRun>(json);
        }
        else
        {
            // Ha nincs mentett adat, lehet �j CurrentRun l�trehoz�sa itt
            saveData.currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
        }
    }
    public void SaveHighScores()
    {
        // P�ld�ul a CurrentRun adatok ment�se JSON form�tumban
        string json = JsonUtility.ToJson(saveData.highScoresData);
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save(); // Mentj�k az adatokat
    }
    public void SaveSettings()
    {
        // P�ld�ul a CurrentRun adatok ment�se JSON form�tumban
        string json = JsonUtility.ToJson(saveData.settingsData);
        PlayerPrefs.SetString("Settings", json);
        PlayerPrefs.Save(); // Mentj�k az adatokat
    }

}
