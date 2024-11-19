using System.Collections.Generic;
using System.Linq;
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
        // Keressük meg az adott játékos rekordját
        var existingHS = saveData.highScoresData.FirstOrDefault(hs => hs.PlayerName == newHS.PlayerName);

        // Ha nincs rekordja, vagy jobb score
        if (existingHS == null || newHS.Score > existingHS.Score)
        {
            if (existingHS != null)
            {
                // Ha már van rekord, azt eltávolítjuk
                saveData.highScoresData.Remove(existingHS);
            }

            // Új rekord hozzáadása
            saveData.highScoresData.Add(newHS);

            // Rendezés idõ szerint (növekvõ sorrendben)
            saveData.highScoresData.Sort((a, b) => a.Score.CompareTo(b.Score));
        }
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
            // Alapértelmezett értékeket állítunk be, ha nincs mentett adat
            saveData.currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
            saveData.settingsData = new Settings(); // vagy alapértelmezett értékek a beállításokhoz
            saveData.highScoresData = new List<HighScore>();
        }
    }
}
