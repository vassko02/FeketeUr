using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPlanets : MonoBehaviour
{
    public List<GameObject> planetPrefabs;  // Ide jönnek a bolygó prefabeid
    public double spawnRate = 8;            // A spawnolás gyakorisága
    private float timer = 0;

    public float minSize = 0.3f;  // Minimum méret szorzó
    public float maxSize = 1f;    // Maximum méret szorzó

    // Start is called before the first frame update
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

    // A bolygók spawnolása
    void spawn()
    {

        float leftPoint = -11f;
        float rightPoint = 11f;

        // Véletlenszerûen kiválasztunk egy bolygó prefab-et a listából
        int randomIndex = Random.Range(0, planetPrefabs.Count);
        GameObject planet = planetPrefabs[randomIndex];

        float randomSize = Random.Range(minSize, maxSize);


        // Instantiáljuk a bolygót egy véletlenszerû pozícióba és forgással
        if (planet != null)
        {
            GameObject newPlanet =Instantiate(planet, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            newPlanet.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

        }
    }
}