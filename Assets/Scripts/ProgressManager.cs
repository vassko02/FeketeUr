using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;


public class ProgressManager : MonoBehaviour
{
    public Player player;

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
            LoadData();

        }
        ToggleSpawner(enemySpawnerObject, false);
        ToggleSpawner(asteroidSpawnerObject, false);
        ToggleSpawner(buffSpawnerObject, true);
        buffSpawner.spawnRate = 8f;
        StartCoroutine(Progress());
    }

    private void LoadData()
    {
        float timeFromLoad = SaveManager.Instance.saveData.currentRunData.ElapsedTime;
        for (int i = obstacleSpawnTimes.Count - 1; i > 0; i--)
        {
            if (timeFromLoad > obstacleSpawnTimes[i])
            {
                elapsedTime = obstacleSpawnTimes[i] - 1;
                break;
            }
        }
    }

    private IEnumerator Progress()
    {
        if (elapsedTime>=0f&&elapsedTime<90f)
        {
            enemySpawner.enemyLimit = 2;
            enemySpawner.spawnColor = Color.green;
        }
        else if (elapsedTime >= 90f && elapsedTime < 180f)
        {
            enemySpawner.enemyLimit = 3;
            enemySpawner.spawnColor = new Color(1f, 0.5f, 0f); // Narancssárga szín
        }
        else if (elapsedTime >= 180f && elapsedTime < 270f)
        {
            enemySpawner.spawnColor = new Color(0.5f, 0f, 0.5f); // Lila szín
            enemySpawner.enemyLimit = 4;
        }
        while (elapsedTime < totalTimerDuration)
        {
            // Idõzített események különbözõ idõpontokban
            if (elapsedTime == obstacleSpawnTimes[0])
            {
                if (doneWithDialog!=true)
                {
                    midDialog = true;
                    StartDialog("#TALK1", dialogIcons[1]);
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
                ToggleSpawner(asteroidSpawnerObject, true);
                asteroidSpawner.asteroidsPerSpawn = 2;
            }
            else if (elapsedTime == obstacleSpawnTimes[2])
            {
                ToggleSpawner(asteroidSpawnerObject, true);
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
                ToggleSpawner(enemySpawnerObject, true);
                enemySpawner.enemiesPerSpawn = 2;
                enemySpawner.spawnRate = 4f;
            }
            else if (elapsedTime == obstacleSpawnTimes[5])
            {
                ToggleSpawner(enemySpawnerObject, true);
                ToggleSpawner(asteroidSpawnerObject,true);
                asteroidSpawner.spawnRate = 2f;
            }
            else if (elapsedTime == obstacleSpawnTimes[6])
            {
                ToggleSpawner(enemySpawnerObject, false);
                ToggleSpawner(asteroidSpawnerObject, true);
                asteroidSpawner.asteroidsPerSpawn = 3;
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
            else if (elapsedTime== obstacleSpawnTimes[9])
            {
                
                //DIALOG A BOSS UTÁN DIALOG
                if (doneWithDialog == false)
                {
                    midDialog = true;
                    StartDialog("#AFTERKRONIS", dialogIcons[2]);
                   
                }
                else if (midDialog == false && doneWithDialog == true) 
                {
                    enemySpawner.spawnColor = new Color(1f, 0.5f, 0f); // Narancssárga szín
                    enemySpawner.enemyLimit = 3;
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
            }
            else if (elapsedTime== obstacleSpawnTimes[11])
            {
                ToggleSpawner(enemySpawnerObject, true);
                ToggleSpawner(asteroidSpawnerObject, true);
                asteroidSpawner.asteroidsPerSpawn = 2;
                asteroidSpawner.spawnRate = 3f;
            }
            else if(elapsedTime== obstacleSpawnTimes[12])
            {
                ToggleSpawner(asteroidSpawnerObject, true);
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
            else if (elapsedTime== obstacleSpawnTimes[15])
            {
                //MÁSODIK A BOSS UTÁN DIALOG
                if (doneWithDialog == false)
                {
                    midDialog = true;
                    StartDialog("#AFTERORION", dialogIcons[3]);

                }
                else if (midDialog == false && doneWithDialog == true)
                {
                    enemySpawner.spawnColor = new Color(0.5f, 0f, 0.5f); // Lila szín
                    enemySpawner.enemyLimit = 4;
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
                ToggleSpawner(asteroidSpawnerObject, true);
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
                if (doneWithDialog != true)
                {
                    midDialog = true;
                    StartDialog("#PRENYX", dialogIcons[4]);

                }
                if (midDialog == false && doneWithDialog == true)
                {
                    bosses[2].SetActive(true);
                }
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
                    yourScore.text = scoreUI.text;

                    healthBar.SetActive(false);
                    scoreUI.gameObject.SetActive(false);

                    winScreen.SetActive(true);
                    Time.timeScale = 0f;

                    HighScore newHS = new HighScore(player.playerName, player.score);

                    SaveManager.Instance.saveData.currentRunData = null;
                    SaveManager.Instance.AddHighScore(newHS);
                    SaveManager.Instance.Save();
                }
            }
            if (!midBossFight&&!midDialog)
            {
                elapsedTime += 1f;
            }
            yield return new WaitForSeconds(1f);
            //Debug.Log(elapsedTime);
        }

    }
    private void StartDialog(string key,GameObject leftCharacter)
    {
        dialogManager.rightCharacter = dialogIcons[0];
        dialogManager.leftCharacter = leftCharacter;
        dialogManager.currentTalkKey = key;
        dialogManagerObject.SetActive(true);
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
