using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPlanets : MonoBehaviour
{
    public List<GameObject> planetPrefabs;  // Ide j�nnek a bolyg� prefabeid
    public double spawnRate = 8;            // A spawnol�s gyakoris�ga
    private float timer = 0;

    public float minSize = 0.3f;  // Minimum m�ret szorz�
    public float maxSize = 1f;    // Maximum m�ret szorz�

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

    // A bolyg�k spawnol�sa
    void spawn()
    {

        float leftPoint = -11f;
        float rightPoint = 11f;

        // V�letlenszer�en kiv�lasztunk egy bolyg� prefab-et a list�b�l
        int randomIndex = Random.Range(0, planetPrefabs.Count);
        GameObject planet = planetPrefabs[randomIndex];

        float randomSize = Random.Range(minSize, maxSize);


        // Instanti�ljuk a bolyg�t egy v�letlenszer� poz�ci�ba �s forg�ssal
        if (planet != null)
        {
            GameObject newPlanet =Instantiate(planet, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            newPlanet.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

        }
    }
}