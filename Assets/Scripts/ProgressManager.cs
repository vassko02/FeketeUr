using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;


public class ProgressManager : MonoBehaviour
{
    public GameObject buffSpawnerObject;
    public GameObject enemySpawnerObject;
    public GameObject asteroidSpawnerObject;
    public GameObject dialogManagerObject;

    private SpawnBuffs buffSpawner;
    private SpawnEnemy enemySpawner;
    private SpawnAsteroid asteroidSpawner;
    private DialogManager dialogManager;

    private float totalTimerDuration = 600f; // Teljes idõtartam
    public float elapsedTime = 0f;
    public List<GameObject> bosses;

    public bool midBossFight = false;

    public GameObject healthBar;
    public Text scoreUI;
    public Text yourScore;
    public GameObject winScreen;
    public List<GameObject> dialogIcons;
    public bool midDialog;
    public bool doneWithDialog=false;
    public bool doneWithBoss=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        buffSpawner = buffSpawnerObject.GetComponent<SpawnBuffs>();
        enemySpawner = enemySpawnerObject.GetComponent<SpawnEnemy>();
        asteroidSpawner = asteroidSpawnerObject.GetComponent<SpawnAsteroid>();
        dialogManager= dialogManagerObject.GetComponent<DialogManager>();
        if (PlayerPrefs.GetInt("continue")==1)
        {
            elapsedTime = PlayerPrefs.GetFloat("progress");

        }
        ToggleSpawner(enemySpawnerObject, false);
        ToggleSpawner(asteroidSpawnerObject, false);
        ToggleSpawner(buffSpawnerObject, false);
        StartCoroutine(Progress());
    }
    private IEnumerator Progress()
    {
        while (elapsedTime < totalTimerDuration)
        {
            // Idõzített események különbözõ idõpontokban
            if (elapsedTime == 0f)
            {
                if (doneWithDialog!=true)
                {
                    midDialog = true;
                    StartDialog("#TALK1", dialogIcons[3]);
                }
                if (midDialog==false&&doneWithDialog==true) 
                {
                    enemySpawner.enemyLimit = 2;
                    ToggleSpawner(buffSpawnerObject, true);
                    ToggleSpawner(enemySpawnerObject, false);
                    ToggleSpawner(asteroidSpawnerObject, true);
                    enemySpawner.spawnColor = Color.green;
                    asteroidSpawner.spawnRate = 1f;
                    asteroidSpawner.asteroidsPerSpawn = 1;
                }
            }
            else if (elapsedTime == 10f )
            {
                doneWithDialog = false;
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
                DestroyAllEnemies();
                midBossFight = true;
                //ELSÕ BOSS DIALOG
                if (doneWithDialog != true)
                {
                    midDialog = true;
                    StartDialog("#PREKRONIS", dialogIcons[2]);
                  
                }
                if (midDialog == false && doneWithDialog == true)
                {
                    bosses[0].SetActive(true);
                }
            }
            else if (elapsedTime==91f)
            {
                
                //DIALOG A BOSS UTÁN DIALOG
                if (doneWithDialog == false)
                {
                    midDialog = true;
                    StartDialog("#AFTERKRONIS", dialogIcons[2]);
                   
                }
                else if (midDialog == false && doneWithDialog == true) 
                {
                    enemySpawner.spawnColor = new Color(0.5f, 0f, 0.5f); // Lila szín
                    enemySpawner.enemyLimit = 3;
                    buffSpawner.spawnRate = 15f;
                    ToggleSpawner(asteroidSpawnerObject, true);
                    asteroidSpawner.asteroidsPerSpawn = 3;
                    asteroidSpawner.spawnRate = 1.5f;
                }
            }
            else if (elapsedTime==110f)
            {
                doneWithDialog = false;
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
                DestroyAllEnemies();
                midBossFight = true;

                //MÁSODIK BOSS DIALOG
                if (doneWithDialog != true)
                {
                    midDialog = true;
                    StartDialog("#PREORION", dialogIcons[3]);

                }
                if (midDialog == false && doneWithDialog == true)
                {
                    bosses[1].SetActive(true);
                }

            }
            else if (elapsedTime==181f)
            {
                //MÁSODIK A BOSS UTÁN DIALOG
                if (doneWithDialog == false)
                {
                    midDialog = true;
                    StartDialog("#AFTERORION", dialogIcons[3]);

                }
                else if (midDialog == false && doneWithDialog == true)
                {
                    enemySpawner.spawnColor = Color.white;
                    enemySpawner.enemyLimit = 4;
                    buffSpawner.spawnRate = 6f;
                    ToggleSpawner(enemySpawnerObject, true);
                    enemySpawner.enemiesPerSpawn = 2;
                    enemySpawner.spawnRate = 3f;
                    ToggleSpawner(asteroidSpawnerObject, true);
                    asteroidSpawner.spawnRate = 1f;
                    asteroidSpawner.asteroidsPerSpawn = 2;
                }
            }
            else if (elapsedTime == 200f)
            {
                doneWithDialog = false;
                ToggleSpawner(enemySpawnerObject,false);
                asteroidSpawner.asteroidsPerSpawn= 5;
            }
            else if (elapsedTime == 220f)
            {
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.enemiesPerSpawn = 3;
                enemySpawner.spawnRate = 3f;
            }
            else if(elapsedTime==250f)
            {
                ToggleSpawner(enemySpawnerObject,false);
                ToggleSpawner(asteroidSpawnerObject,true);
                asteroidSpawner.asteroidsPerSpawn = 4;
                asteroidSpawner.spawnRate = 3f;
            }
            else if (elapsedTime == 270f)
            {
                ToggleSpawner(enemySpawnerObject, false);
                ToggleSpawner(asteroidSpawnerObject, false);
                DestroyAllEnemies();
                midBossFight = true;
                //HARMADIK BOSS DIALOG
                if (doneWithDialog != true)
                {
                    midDialog = true;
                    StartDialog("#PRENYX", dialogIcons[4]);

                }
                if (midDialog == false && doneWithDialog == true)
                {
                    bosses[2].SetActive(true);
                }

                //HARMADIK A BOSS UTÁN DIALOG

            }
            // Egy másodperc várakozás
            else if (elapsedTime==271f)
            {
                if (doneWithDialog == false)
                {
                    midDialog = true;
                    StartDialog("#AFTERNYX", dialogIcons[4]);

                }
                else if (midDialog == false && doneWithDialog == true)
                {
                    PlayerPrefs.SetInt("alive", 0);
                    yourScore.text = scoreUI.text;

                    healthBar.SetActive(false);
                    scoreUI.gameObject.SetActive(false);

                    winScreen.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
            if (!midBossFight&&!midDialog)
            {
                elapsedTime += 1f;
            }
            yield return new WaitForSeconds(1f);
            Debug.Log(elapsedTime);
        }

    }

    private void StartDialog(string key,GameObject leftCharacter)
    {
        
        dialogManager.rightCharacter = dialogIcons[0];
        dialogManager.leftCharacter = leftCharacter;
        dialogManager.currentTalkKey = key;
        dialogManagerObject.SetActive(true);
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
     void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
