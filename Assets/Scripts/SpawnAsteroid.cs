using UnityEngine;
using System.Collections;

public class SpawnAsteroid : MonoBehaviour
{
    public GameObject asteroid1;
    public GameObject asteroid2;
    public GameObject asteroid3;

    public double spawnRate = 0.5;
    private float timer = 0;

    // �j param�ter: H�ny aszteroid�t spawnol egyszerre
    public int asteroidsPerSpawn = 2;

    void Start()
    {
        spawn();  // Els� spawn ind�t�skor
    }

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawn();  // Spawnol�s id�k�z�nk�nt
            timer = 0;
        }
    }

    void spawn()
    {
        float leftPoint = -10;
        float rightPoint = 10;

        // Spawnol�s az asteroidsPerSpawn param�ter szerint
        for (int i = 0; i < asteroidsPerSpawn; i++)
        {
            int id = Random.Range(1, 4);  // V�letlenszer�en kiv�laszt egy aszteroid�t
            GameObject asteroid = null;

            switch (id)
            {
                case 1:
                    asteroid = asteroid1;
                    break;
                case 2:
                    asteroid = asteroid2;
                    break;
                case 3:
                    asteroid = asteroid3;
                    break;
                default:
                    break;
            }

            if (asteroid != null)
            {
                // Instanci�lja az aszteroid�t v�letlenszer� helyen �s forgat�ssal
                Instantiate(asteroid, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            }
        }
    }
}