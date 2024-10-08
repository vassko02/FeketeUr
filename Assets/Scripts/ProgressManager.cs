using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class ProgressManager : MonoBehaviour
{
    public GameObject buffSpawnerObject;
    public GameObject enemySpawnerObject;
    public GameObject asteroidSpawnerObject;

    private SpawnBuffs buffSpawner;
    private SpawnEnemy enemySpawner;
    private SpawnAsteroid asteroidSpawner;

    private float totalTimerDuration = 600f; // Teljes idõtartam
    public float elapsedTime = 0f;

    private bool midBossFight = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buffSpawner = buffSpawnerObject.GetComponent<SpawnBuffs>();
        enemySpawner = enemySpawnerObject.GetComponent<SpawnEnemy>();
        asteroidSpawner = asteroidSpawnerObject.GetComponent<SpawnAsteroid>();

        if (PlayerPrefs.GetInt("continue")==1)
        {
            elapsedTime = PlayerPrefs.GetFloat("progress",0);

        }
        StartCoroutine(Progress());

    }
    private IEnumerator Progress()
    {
        while (elapsedTime < totalTimerDuration)
        {
            // Idõzített események különbözõ idõpontokban
            if (elapsedTime == 0f)
            {
                enemySpawner.enemyLimit = 3;
                ToggleSpawner(buffSpawnerObject,true);
                ToggleSpawner(enemySpawnerObject, false);
                ToggleSpawner(asteroidSpawnerObject, true);
                enemySpawner.spawnColor = Color.red;
                asteroidSpawner.spawnRate = 1f;
                asteroidSpawner.asteroidsPerSpawn = 1;

            }
            else if (elapsedTime == 10f )
            {
                asteroidSpawner.asteroidsPerSpawn = 2;
            }
            else if (elapsedTime == 15f)
            {
                asteroidSpawner.spawnRate = 0.5f;
            }
            else if(elapsedTime == 25f)
            {
                ResetAsteroidSpawner();
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.spawnRate = 2f;
            }
            else if (elapsedTime==35f)
            {
                enemySpawner.enemiesPerSpawn = 2;
                enemySpawner.spawnRate = 4f;
                buffSpawner.spawnRate = 11f;
            }
            else if (elapsedTime == 45f)
            {
                ToggleSpawner(asteroidSpawnerObject,true);
                asteroidSpawner.spawnRate = 2f;
            }
            else if (elapsedTime == 60f)
            {
                ToggleSpawner(enemySpawnerObject, false);
                asteroidSpawner.asteroidsPerSpawn = 3;
                buffSpawner.spawnRate = 7f;
            }
            else if (elapsedTime == 75f)
            {
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject, true);
                enemySpawner.spawnRate = 2f;
                enemySpawner.enemiesPerSpawn = 2;
            }

            else if (elapsedTime==90f)
            {
                ToggleSpawner(enemySpawnerObject,false); 
                ToggleSpawner(asteroidSpawnerObject, false);

                midBossFight = true;
                //ELSÕ BOSS DIALOG

                //ELSÕ BOSSFIGHT

                //DIALOG A BOSS UTÁN

            }

            else if (elapsedTime == 180f)
            {
                midBossFight = true;
                //MÁSODIK BOSS DIALOG

                //MÁSODIK BOSSFIGHT

                //MÁSODIK A BOSS UTÁN

            }

            else if (elapsedTime == 270f)
            {
                midBossFight = true;
                //MÁSODIK BOSS DIALOG

                //MÁSODIK BOSSFIGHT

                //MÁSODIK A BOSS UTÁN

            }
            // Egy másodperc várakozás
            if (!midBossFight)
            {
                elapsedTime += 1f;
            }
            yield return new WaitForSeconds(1f);
            Debug.Log(elapsedTime);
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
    void ResetAsteroidSpawner()
    {
        asteroidSpawner.spawnRate = 1f;
        asteroidSpawner.asteroidsPerSpawn = 1;
    }
    void ResetBuffSpawner()
    {
        buffSpawner.spawnRate = 30f;

    }
    void ResetEnemySpawner()
    {
        enemySpawner.spawnRate= 1f;
        enemySpawner.enemiesPerSpawn = 1;
    }
    void ToggleSpawner(GameObject spawner, bool active)
    {
        spawner.SetActive(active);
    }
}
