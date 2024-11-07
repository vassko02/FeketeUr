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

    }
    public void Save()
    {
        string json = JsonUtility.ToJson(Instance.saveData, true);
        PlayerPrefs.SetString("GameData", json);
        Debug.Log(json);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string json = PlayerPrefs.GetString("GameData");
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Loaded"+json);
        }
        else
        {
            // Alap�rtelmezett �rt�keket �ll�tunk be, ha nincs mentett adat
            saveData.currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
            saveData.settingsData = new Settings(); // vagy alap�rtelmezett �rt�kek a be�ll�t�sokhoz
            saveData.highScoresData = new List<HighScore>();
        }
    }
}
