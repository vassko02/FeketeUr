using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    // High Scores, Settings, és a Current Session Data
    public List<HighScore> highScores = new List<HighScore>();
    public Dictionary<string, object> settings = new Dictionary<string, object>();
    public CurrentRun currentRun;
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
        highScores.Add(newHS);
        highScores.Sort((a, b) => b.Score.CompareTo(a.Score)); // Descending order
        if (highScores.Count > 10) // Keep only top 10 scores
        {
            highScores.RemoveAt(highScores.Count - 1);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
