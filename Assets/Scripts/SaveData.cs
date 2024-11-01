using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class SaveData
{
    public CurrentRun currentRunData = new CurrentRun();
    public Settings settingsData = new Settings();
    public List<HighScore> highScoresData = new List<HighScore>();

    public SaveData(CurrentRun currentRun, Settings settings, List<HighScore> highScores)
    {
        this.currentRunData = currentRun;
        this.settingsData = settings;
        this.highScoresData = highScores;
    }
    public SaveData()
    {
        currentRunData = new CurrentRun();
        settingsData = new Settings();
        highScoresData = new List<HighScore>();
    }
}