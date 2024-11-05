using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class SaveManagerTest
{
    private SaveManager saveManager;

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy új GameObjectet, és hozzárendeljük a SaveManager komponenst
        GameObject saveManagerObject = new GameObject();
        saveManager = saveManagerObject.AddComponent<SaveManager>();

        saveManager.saveData = new SaveData
        {
            highScoresData = new List<HighScore>{},
            currentRunData = new CurrentRun("Player1", 1, 100, 100, 10, 0f),
            settingsData = new Settings() // Alapértelmezett beállításokat adunk hozzá
        };
    }
    [Test]
    public void Save_SuccessfullySavesData()
    {
        // Meghívjuk a Save metódust, hogy mentse a `saveData`-t a PlayerPrefs-be
        string json = JsonUtility.ToJson(saveManager.saveData, true);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
        // Ellenõrizzük, hogy a mentett JSON megfelel-e a `saveData`-nak
        string savedJson = PlayerPrefs.GetString("GameData");
        SaveData loadedData = JsonUtility.FromJson<SaveData>(savedJson);
        // Ellenõrizzük a current run adatokat
        Assert.AreEqual("Player1", loadedData.currentRunData.PlayerName);
        Assert.AreEqual(100, loadedData.currentRunData.CurrentHealth);

        // Ellenõrizzük a settings adatokat
        Assert.IsNotNull(loadedData.settingsData, "Settings data should not be null.");
    }
    [Test]
    public void AddHighScore_AddsNewHighScore()
    {
        // Új high score létrehozása
        HighScore newHighScore = new HighScore("TestPlayer", 1000);

        // Hozzáadjuk a high score-t
        saveManager.AddHighScore(newHighScore);

        // Ellenõrizzük, hogy a high score bekerült a listába
        Assert.Contains(newHighScore, saveManager.saveData.highScoresData);
        Assert.AreEqual(1, saveManager.saveData.highScoresData.Count, "High score list should contain one entry.");
    }
    [Test]
    public void Load_SuccessfullyLoadsData()
    {
        // Elõre definiált SaveData adat létrehozása
        SaveData predefinedData = new SaveData
        {
            highScoresData = new List<HighScore>
            {
                new HighScore("Player1", 500),
                new HighScore("Player2", 1000)
            },
            currentRunData = new CurrentRun("Player1", 1, 100, 100, 10, 0f),
            settingsData = new Settings() // Felvehetünk alapértelmezett értékeket a Settings-hez
        };

        // JSON formátumba konvertáljuk, és elmentjük a PlayerPrefs-be
        string json = JsonUtility.ToJson(predefinedData);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();

        // Meghívjuk a Load metódust
        saveManager.Load();

        // Ellenõrzések
        Assert.AreEqual(2, saveManager.saveData.highScoresData.Count, "High scores list should contain two entries.");
        Assert.AreEqual("Player1", saveManager.saveData.highScoresData[0].PlayerName);
        Assert.AreEqual(500, saveManager.saveData.highScoresData[0].Score);
        Assert.AreEqual("Player2", saveManager.saveData.highScoresData[1].PlayerName);
        Assert.AreEqual(1000, saveManager.saveData.highScoresData[1].Score);

        Assert.AreEqual("Player1", saveManager.saveData.currentRunData.PlayerName);

        // Beállítások ellenõrzése (ha vannak alapértelmezett beállítások)
        Assert.IsNotNull(saveManager.saveData.settingsData, "Settings data should not be null.");
    }

    [TearDown]
    public void TearDown()
    {
        // Töröljük a PlayerPrefs-et a teszt után, hogy ne zavarjon be más teszteknél
        PlayerPrefs.DeleteKey("GameData");
    }
}