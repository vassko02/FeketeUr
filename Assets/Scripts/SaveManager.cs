using System.Collections.Generic;
using System.Linq;
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
        // Keress�k meg az adott j�t�kos rekordj�t
        var existingHS = saveData.highScoresData.FirstOrDefault(hs => hs.PlayerName == newHS.PlayerName);

        // Ha nincs rekordja, vagy jobb score
        if (existingHS == null || newHS.Score > existingHS.Score)
        {
            if (existingHS != null)
            {
                // Ha m�r van rekord, azt elt�vol�tjuk
                saveData.highScoresData.Remove(existingHS);
            }

            // �j rekord hozz�ad�sa
            saveData.highScoresData.Add(newHS);

            // Rendez�s id� szerint (n�vekv� sorrendben)
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
            // Alap�rtelmezett �rt�keket �ll�tunk be, ha nincs mentett adat
            saveData.currentRunData = new CurrentRun("Player", 0, 100, 100, 5, 0f);
            saveData.settingsData = new Settings(); // vagy alap�rtelmezett �rt�kek a be�ll�t�sokhoz
            saveData.highScoresData = new List<HighScore>();
        }
    }
}
