using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject bulletPrefabRight; // A l�ved�k prefabja
    public GameObject bulletPrefabLeft; // A l�ved�k prefabja
    public float moveSpeed = 5f; // Player mozg�si sebess�ge
    public float fireRate = 0.5f; // L�v�sek k�z�tti id�k�z

    private Vector2 screenBounds; // A k�perny� sz�lei
    private float objectWidth;
    private float objectHeight;
    private float nextFireTime = 0f;
    private AudioSource audioSource; //L�v�s hang

    public int maxHealt = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject gameOverScreen;

    void Start()
    {
        // A k�perny� sz�leinek kisz�m�t�sa
        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        // A haj� m�ret�nek kisz�m�t�sa
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealt;
        healthBar.SetMaxHealth(maxHealt);

    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0);
        transform.position += move * moveSpeed * Time.deltaTime;

        // A j�t�kos k�perny�n bel�l tart�sa
        keepPlayerInBounds();

        // L�v�s
        if (Input.GetMouseButtonDown(0)) // Bal eg�rgomb lenyom�sa
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        void keepPlayerInBounds()
        {
            // J�t�kos aktu�lis poz�ci�ja
            Vector3 pos = transform.position;

            // Poz�ci� korl�toz�sa a k�perny�n bel�lre
            pos.x = Mathf.Clamp(pos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
            pos.y = Mathf.Clamp(pos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);

            // Poz�ci� firss�t�se
            transform.position = pos;
        }

        // L�v�s
        void Shoot()
        {
            // L�ved�kek l�trehoz�sa
            float offset = 0.4f;
            GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));
        }

    }
    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
        }
        else
        {
            currentHealth -= damage;

            GameObject enemySpawner = GameObject.FindWithTag("EnemySpawner");
            GameObject buffSpawner = GameObject.FindWithTag("EnemySpawner");

            if (enemySpawner != null)
            {
                enemySpawner.SetActive(false);
            }
            if (buffSpawner != null)
            {
                buffSpawner.SetActive(false);
            }
            Destroy(gameObject);
            gameOverScreen.SetActive(true);

        }
        healthBar.setHealth(currentHealth);
    }
    public void Heal(int heal)
    {
        if (currentHealth + heal > maxHealt)
        {
            currentHealth = maxHealt;
        }
        else
        {
            currentHealth += heal;
        }
        healthBar.setHealth(currentHealth);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            TakeDamage(20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            TakeDamage(25);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(35);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("HealBuff"))
        {
            Heal(20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("DamageBuff"))
        {


            Destroy(collision.gameObject);
        }
    }
}