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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buffSpawner = buffSpawnerObject.GetComponent<SpawnBuffs>();
        enemySpawner = enemySpawnerObject.GetComponent<SpawnEnemy>();
        asteroidSpawner = asteroidSpawnerObject.GetComponent<SpawnAsteroid>();

        Debug.Log(elapsedTime);

        StartCoroutine(Progress());

    }
    private IEnumerator Progress()
    {
        while (elapsedTime < totalTimerDuration)
        {
            // Idõzített események különbözõ idõpontokban
            if (elapsedTime ==5f)
            {
                Debug.Log("5sec");
            }
            else if (elapsedTime == 10f )
            {
                Debug.Log("10sec");

            }
            else if (elapsedTime >= 20f)
            {

            }

            // Egy másodperc várakozás
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

    void ToggleSpawner(GameObject spawner, bool active)
    {
        spawner.SetActive(active);
    }
}
