using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnAsteroid : MonoBehaviour
{
    public GameObject asteroid;
    public double spawnRate = 0.5;
    private float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawn();
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
    void spawn()
    {
        float leftPoint = -10;
        float rightPoint = 10;
        Instantiate(asteroid, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

    }
}
