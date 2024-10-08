using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject enemy;

    public double spawnRate = 3;
    public int enemiesPerSpawn = 1;
    private float timer = 0;
    public Color spawnColor = Color.red;
    public int enemyLimit;
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
    void spawn()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount<enemyLimit) {
            float leftPoint = -10;
            float rightPoint = 10;
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(leftPoint, rightPoint), transform.position.y, 0), Quaternion.Euler(0, 0, 180));
                ChangeEnemyColor(newEnemy, spawnColor);
                Enemy enemyScript = newEnemy.GetComponent<Enemy>();
                enemyScript.color = spawnColor;
            }
            void ChangeEnemyColor(GameObject enemy, Color newColor)
            {
                SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();
                if (enemyRenderer != null)
                {
                    enemyRenderer.color = newColor;
                }
                else
                {
                    Debug.LogWarning("Nem található Renderer komponens az ellenségen!");
                }
            }
        }


    }
}
