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
    public List<GameObject> bosses;

    public bool midBossFight = false;
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
                enemySpawner.spawnColor = Color.green;
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

                bosses[0].SetActive(true);

                //DIALOG A BOSS UTÁN DIALOG

            }
            else if (elapsedTime==91f)
            {
                enemySpawner.spawnColor = new Color(0.5f, 0f, 0.5f); // Lila szín
                enemySpawner.enemyLimit = 4;
                buffSpawner.spawnRate = 15f;
                ToggleSpawner(asteroidSpawnerObject, true);
                asteroidSpawner.asteroidsPerSpawn = 3;
                asteroidSpawner.spawnRate = 1.5f;
            }
            else if (elapsedTime==110f)
            {
                ToggleSpawner(asteroidSpawnerObject,false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.spawnRate = 3f;
                enemySpawner.enemiesPerSpawn = 2;
                buffSpawner.spawnRate = 10f;
            }
            else if (elapsedTime==125f)
            {
                ToggleSpawner(asteroidSpawnerObject, true);
                asteroidSpawner.asteroidsPerSpawn = 2;
                asteroidSpawner.spawnRate = 3f;
            }
            else if(elapsedTime==135f)
            {
                ToggleSpawner(enemySpawnerObject, false);
                asteroidSpawner.spawnRate = 2f;
            }
            else if (elapsedTime == 155f)
            {
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.spawnRate = 1f;
                enemySpawner.enemiesPerSpawn = 1;
            }
            else if (elapsedTime == 180f)
            {
                ToggleSpawner(enemySpawnerObject, false);
                ToggleSpawner(asteroidSpawnerObject, false);

                midBossFight = true;

                //MÁSODIK BOSS DIALOG

                bosses[1].SetActive(true);
                //MÁSODIK A BOSS UTÁN DIALOG

                midBossFight = false;
            }
            else if (elapsedTime==181)
            {
                enemySpawner.spawnColor = Color.yellow;
                enemySpawner.enemyLimit = 5;
                buffSpawner.spawnRate = 20f;
                ToggleSpawner(enemySpawnerObject, true);
                ToggleSpawner(asteroidSpawnerObject,false);
            }
            else if (elapsedTime == 270f)
            {
                midBossFight = true;
                //HARMADIK BOSS DIALOG

                //HARMADIK BOSSFIGHT

                //HARMADIK A BOSS UTÁN DIALOG

                //WIN SCREEN

                midBossFight = false;
            }
            // Egy másodperc várakozás
            if (!midBossFight)
            {
                elapsedTime += 1f;
            }
            yield return new WaitForSeconds(1f);
            //Debug.Log(elapsedTime);
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
