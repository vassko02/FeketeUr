using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnBuffs : MonoBehaviour
{
    public List<GameObject> buffPrefabs;  
    public double spawnRate = 8;            // A spawnolás gyakorisága
    private float timer = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawn();
            timer = 0;
        }
    }

    // A bolygók spawnolása
    void spawn()
    {
        float leftPoint = -9f;
        float rightPoint = 9f;

        // Véletlenszerûen kiválasztunk egy  prefab-et a listából
        int randomIndex = Random.Range(0, buffPrefabs.Count);
        GameObject buff = buffPrefabs[randomIndex];

        if (buff != null)
        {
            GameObject newBuff = Instantiate(buff, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), Quaternion.Euler(0, 0, 0));

        }
    }
}
