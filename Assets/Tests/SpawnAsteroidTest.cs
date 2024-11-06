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
        // L�trehozzuk a SpawnAsteroid GameObjectet �s hozz�rendelj�k a scriptet
        spawnAsteroidObject = new GameObject("SpawnAsteroid");
        spawnAsteroidScript = spawnAsteroidObject.AddComponent<SpawnAsteroid>();

        // L�trehozunk dummy aszteroid�kat
        spawnAsteroidScript.asteroid1 = new GameObject("Asteroid1");
        spawnAsteroidScript.asteroid1.gameObject.tag = "Asteroid";
        spawnAsteroidScript.asteroid2 = new GameObject("Asteroid2");
        spawnAsteroidScript.asteroid2.gameObject.tag = "Asteroid";
        spawnAsteroidScript.asteroid3 = new GameObject("Asteroid3");
        spawnAsteroidScript.asteroid3.gameObject.tag = "Asteroid";
        // Be�ll�tjuk a spawnRate-ot �s az aszteroid�k sz�m�t
        spawnAsteroidScript.spawnRate = 0.1f;
        spawnAsteroidScript.asteroidsPerSpawn = 3;
    }

    [Test]
    public void TestAsteroidSpawn()
    {
        // Figyelj�k meg, hogy a spawn ut�n megfelel� sz�m� aszteroida j�n-e l�tre
        int initialAsteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Count();

        spawnAsteroidScript.spawn();

        int newAsteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Count();

        // Az �j aszteroid�k sz�m�nak n�veked�se, figyelembe v�ve az aszteroid�k sz�m�t, amit spawnoltunk
        Assert.Greater(newAsteroidCount, initialAsteroidCount);
    }
}
