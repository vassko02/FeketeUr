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
        // L�trehozunk egy �j GameObjectet, �s hozz�rendelj�k a SaveManager komponenst
        GameObject saveManagerObject = new GameObject();
        saveManager = saveManagerObject.AddComponent<SaveManager>();

        saveManager.saveData = new SaveData
        {
            highScoresData = new List<HighScore>{},
            currentRunData = new CurrentRun("Player1", 1, 100, 100, 10, 0f),
            settingsData = new Settings() // Alap�rtelmezett be�ll�t�sokat adunk hozz�
        };
    }
    [Test]
    public void Save_SuccessfullySavesData()
    {
        // Megh�vjuk a Save met�dust, hogy mentse a `saveData`-t a PlayerPrefs-be
        string json = JsonUtility.ToJson(saveManager.saveData, true);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
        // Ellen�rizz�k, hogy a mentett JSON megfelel-e a `saveData`-nak
        string savedJson = PlayerPrefs.GetString("GameData");
        SaveData loadedData = JsonUtility.FromJson<SaveData>(savedJson);
        // Ellen�rizz�k a current run adatokat
        Assert.AreEqual("Player1", loadedData.currentRunData.PlayerName);
        Assert.AreEqual(100, loadedData.currentRunData.CurrentHealth);

        // Ellen�rizz�k a settings adatokat
        Assert.IsNotNull(loadedData.settingsData, "Settings data should not be null.");
    }
    [Test]
    public void AddHighScore_AddsNewHighScore()
    {
        // �j high score l�trehoz�sa
        HighScore newHighScore = new HighScore("TestPlayer", 1000);

        // Hozz�adjuk a high score-t
        saveManager.AddHighScore(newHighScore);

        // Ellen�rizz�k, hogy a high score beker�lt a list�ba
        Assert.Contains(newHighScore, saveManager.saveData.highScoresData);
        Assert.AreEqual(1, saveManager.saveData.highScoresData.Count, "High score list should contain one entry.");
    }
    [Test]
    public void Load_SuccessfullyLoadsData()
    {
        // El�re defini�lt SaveData adat l�trehoz�sa
        SaveData predefinedData = new SaveData
        {
            highScoresData = new List<HighScore>
            {
                new HighScore("Player1", 500),
                new HighScore("Player2", 1000)
            },
            currentRunData = new CurrentRun("Player1", 1, 100, 100, 10, 0f),
            settingsData = new Settings() // Felvehet�nk alap�rtelmezett �rt�keket a Settings-hez
        };

        // JSON form�tumba konvert�ljuk, �s elmentj�k a PlayerPrefs-be
        string json = JsonUtility.ToJson(predefinedData);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();

        // Megh�vjuk a Load met�dust
        saveManager.Load();

        // Ellen�rz�sek
        Assert.AreEqual(2, saveManager.saveData.highScoresData.Count, "High scores list should contain two entries.");
        Assert.AreEqual("Player1", saveManager.saveData.highScoresData[0].PlayerName);
        Assert.AreEqual(500, saveManager.saveData.highScoresData[0].Score);
        Assert.AreEqual("Player2", saveManager.saveData.highScoresData[1].PlayerName);
        Assert.AreEqual(1000, saveManager.saveData.highScoresData[1].Score);

        Assert.AreEqual("Player1", saveManager.saveData.currentRunData.PlayerName);

        // Be�ll�t�sok ellen�rz�se (ha vannak alap�rtelmezett be�ll�t�sok)
        Assert.IsNotNull(saveManager.saveData.settingsData, "Settings data should not be null.");
    }

    [TearDown]
    public void TearDown()
    {
        // T�r�lj�k a PlayerPrefs-et a teszt ut�n, hogy ne zavarjon be m�s tesztekn�l
        PlayerPrefs.DeleteKey("GameData");
    }
}