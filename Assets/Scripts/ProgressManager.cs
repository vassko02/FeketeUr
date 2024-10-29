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

    private float totalTimerDuration = 600f; // Teljes id�tartam
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

    private int index=0;

    public List<float> obstacleSpawnTimes = new List<float> { 0f, 10f, 15f, 25f, 35f, 45f,60f,75f,90f,91f,110f,125f,135f,155f,180f,181f,200f,220,250f,270f,271f };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buffSpawner = buffSpawnerObject.GetComponent<SpawnBuffs>();
        enemySpawner = enemySpawnerObject.GetComponent<SpawnEnemy>();
        asteroidSpawner = asteroidSpawnerObject.GetComponent<SpawnAsteroid>();
        dialogManager= dialogManagerObject.GetComponent<DialogManager>();
        if (PlayerPrefs.GetInt("continue")==1)
        {
            SaveManager.Instance.Load();
            float timeFromLoad = SaveManager.Instance.saveData.currentRunData.ElapsedTime;
            Debug.Log(timeFromLoad);
            for (int i = obstacleSpawnTimes.Count-1; i > 0; i--)
            {
                if (timeFromLoad > obstacleSpawnTimes[i])
                {
                    elapsedTime = obstacleSpawnTimes[i]-1;
                    break;
                }
            }
        }
        ToggleSpawner(enemySpawnerObject, false);
        ToggleSpawner(asteroidSpawnerObject, false);
        ToggleSpawner(buffSpawnerObject, true);
        enemySpawner.enemyLimit = 2;
        enemySpawner.spawnColor = Color.green;
        StartCoroutine(Progress());
    }
    private IEnumerator Progress()
    {
        while (elapsedTime < totalTimerDuration)
        {
            // Id�z�tett esem�nyek k�l�nb�z� id�pontokban
            if (elapsedTime == obstacleSpawnTimes[0])
            {
                if (doneWithDialog!=true)
                {
                    midDialog = true;
                    StartDialog("#TALK1", dialogIcons[3]);
                }
                if (midDialog==false&&doneWithDialog==true) 
                {
                    ToggleSpawner(enemySpawnerObject, false);
                    ToggleSpawner(asteroidSpawnerObject, true);
                    asteroidSpawner.spawnRate = 1f;
                    asteroidSpawner.asteroidsPerSpawn = 1;
                }
            }
            else if (elapsedTime == obstacleSpawnTimes[1])
            {
                doneWithDialog = false;
                asteroidSpawner.asteroidsPerSpawn = 2;
            }
            else if (elapsedTime == obstacleSpawnTimes[2])
            {
                asteroidSpawner.spawnRate = 0.5f;
            }
            else if(elapsedTime == obstacleSpawnTimes[3])
            {
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.spawnRate = 2f;
            }
            else if (elapsedTime== obstacleSpawnTimes[4])
            {
                enemySpawner.enemiesPerSpawn = 2;
                enemySpawner.spawnRate = 4f;
                buffSpawner.spawnRate = 11f;
            }
            else if (elapsedTime == obstacleSpawnTimes[5])
            {
                ToggleSpawner(asteroidSpawnerObject,true);
                asteroidSpawner.spawnRate = 2f;
            }
            else if (elapsedTime == obstacleSpawnTimes[6])
            {
                ToggleSpawner(enemySpawnerObject, false);
                asteroidSpawner.asteroidsPerSpawn = 3;
                buffSpawner.spawnRate = 7f;
            }
            else if (elapsedTime == obstacleSpawnTimes[7])
            {
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject, true);
                enemySpawner.spawnRate = 2f;
                enemySpawner.enemiesPerSpawn = 2;
            }

            else if (elapsedTime== obstacleSpawnTimes[8])
            {
                ToggleSpawner(enemySpawnerObject,false); 
                ToggleSpawner(asteroidSpawnerObject, false);
                DestroyAllEnemies();
                midBossFight = true;
                //ELS� BOSS DIALOG
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
            else if (elapsedTime== obstacleSpawnTimes[9])
            {
                
                //DIALOG A BOSS UT�N DIALOG
                if (doneWithDialog == false)
                {
                    midDialog = true;
                    StartDialog("#AFTERKRONIS", dialogIcons[2]);
                   
                }
                else if (midDialog == false && doneWithDialog == true) 
                {
                    enemySpawner.spawnColor = new Color(0.5f, 0f, 0.5f); // Lila sz�n
                    enemySpawner.enemyLimit = 3;
                    buffSpawner.spawnRate = 15f;
                    ToggleSpawner(asteroidSpawnerObject, true);
                    asteroidSpawner.asteroidsPerSpawn = 3;
                    asteroidSpawner.spawnRate = 1.5f;
                }
            }
            else if (elapsedTime== obstacleSpawnTimes[10])
            {
                doneWithDialog = false;
                ToggleSpawner(asteroidSpawnerObject,false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.spawnRate = 3f;
                enemySpawner.enemiesPerSpawn = 2;
                buffSpawner.spawnRate = 10f;
            }
            else if (elapsedTime== obstacleSpawnTimes[11])
            {
                ToggleSpawner(asteroidSpawnerObject, true);
                asteroidSpawner.asteroidsPerSpawn = 2;
                asteroidSpawner.spawnRate = 3f;
            }
            else if(elapsedTime== obstacleSpawnTimes[12])
            {
                ToggleSpawner(enemySpawnerObject, false);
                asteroidSpawner.spawnRate = 2f;
            }
            else if (elapsedTime == obstacleSpawnTimes[13])
            {
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.spawnRate = 1f;
                enemySpawner.enemiesPerSpawn = 1;
            }
            else if (elapsedTime == obstacleSpawnTimes[14])
            {
                ToggleSpawner(enemySpawnerObject, false);
                ToggleSpawner(asteroidSpawnerObject, false);
                DestroyAllEnemies();
                midBossFight = true;

                //M�SODIK BOSS DIALOG
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
            else if (elapsedTime== obstacleSpawnTimes[15])
            {
                //M�SODIK A BOSS UT�N DIALOG
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
            else if (elapsedTime == obstacleSpawnTimes[16])
            {
                doneWithDialog = false;
                ToggleSpawner(enemySpawnerObject,false);
                asteroidSpawner.asteroidsPerSpawn= 5;
            }
            else if (elapsedTime == obstacleSpawnTimes[17])
            {
                ToggleSpawner(asteroidSpawnerObject, false);
                ToggleSpawner(enemySpawnerObject,true);
                enemySpawner.enemiesPerSpawn = 3;
                enemySpawner.spawnRate = 3f;
            }
            else if(elapsedTime== obstacleSpawnTimes[18])
            {
                ToggleSpawner(enemySpawnerObject,false);
                ToggleSpawner(asteroidSpawnerObject,true);
                asteroidSpawner.asteroidsPerSpawn = 4;
                asteroidSpawner.spawnRate = 3f;
            }
            else if (elapsedTime == obstacleSpawnTimes[19])
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

                //HARMADIK A BOSS UT�N DIALOG

            }
            else if (elapsedTime== obstacleSpawnTimes[20])
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
