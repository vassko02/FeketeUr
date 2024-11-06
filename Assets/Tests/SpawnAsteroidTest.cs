using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

public class SpawnAsteroidTest
{
    private GameObject spawnAsteroidObject;
    private SpawnAsteroid spawnAsteroidScript;

    [SetUp]
    public void Setup()
    {
        // Létrehozzuk a SpawnAsteroid GameObjectet és hozzárendeljük a scriptet
        spawnAsteroidObject = new GameObject("SpawnAsteroid");
        spawnAsteroidScript = spawnAsteroidObject.AddComponent<SpawnAsteroid>();

        // Létrehozunk dummy aszteroidákat
        spawnAsteroidScript.asteroid1 = new GameObject("Asteroid1");
        spawnAsteroidScript.asteroid1.gameObject.tag = "Asteroid";
        spawnAsteroidScript.asteroid2 = new GameObject("Asteroid2");
        spawnAsteroidScript.asteroid2.gameObject.tag = "Asteroid";
        spawnAsteroidScript.asteroid3 = new GameObject("Asteroid3");
        spawnAsteroidScript.asteroid3.gameObject.tag = "Asteroid";
        // Beállítjuk a spawnRate-ot és az aszteroidák számát
        spawnAsteroidScript.spawnRate = 0.1f;
        spawnAsteroidScript.asteroidsPerSpawn = 3;
    }

    [Test]
    public void TestAsteroidSpawn()
    {
        // Figyeljük meg, hogy a spawn után megfelelõ számú aszteroida jön-e létre
        int initialAsteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Count();

        spawnAsteroidScript.spawn();

        int newAsteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Count();

        // Az új aszteroidák számának növekedése, figyelembe véve az aszteroidák számát, amit spawnoltunk
        Assert.Greater(newAsteroidCount, initialAsteroidCount);
    }
}
