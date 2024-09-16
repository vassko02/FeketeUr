using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject enemy;

    public double spawnRate = 3;
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
        Instantiate(enemy, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), Quaternion.Euler(0, 0,180));

    }
}
