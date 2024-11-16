using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabRight; // A l�ved�k prefabja
    public GameObject bulletPrefabLeft; // A l�ved�k prefabja
    public float moveSpeed = 5f; // Player mozg�si sebess�ge
    public float fireRate = 0.5f; // L�v�sek k�z�tti id�k�z
    public float shootRange = 2f;
    private float nextFireTime = 0f;

    private Transform playerTransform; // A player poz�ci�j�nak t�rol�s�ra
    private GameObject player;
    private Vector3 targetPosition;

    public int maxHealt = 100;
    public int currentHealth;

    public float separationRadius = 1.5f; // Minim�lis t�vols�g az ellens�gek k�z�tt
    public LayerMask enemyLayer;
    private Player playerScript;
    public Color color;
    private float cooldownTimer = 0f; // L�v�si id�z�t�


    void Start()
    {
        currentHealth = maxHealt;
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerScript = player.GetComponent<Player>();

        }

    }

    void Update()
    {
        FollowPlayer();
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= fireRate)
        {
            if (player != null)
            {
                // Ellen�rizz�k, hogy a j�t�kos X poz�ci�ja k�zel van-e az ellens�g X poz�ci�j�hoz
                if (Mathf.Abs(player.transform.position.x - transform.position.x) <= shootRange)
                {
                    Shoot();
                    cooldownTimer = 0f; // Sz�ml�l� �jraind�t�sa
                }
            }
        }
    }

    // L�v�s
    public void Shoot()
    {
        // L�ved�kek l�trehoz�sa
        float offset = 0.5f;

        GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + offset, transform.position.y - offset, transform.position.z), Quaternion.Euler(0, 0, -90));
        GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - offset, transform.position.y - offset, transform.position.z), Quaternion.Euler(0, 0, -90));

        // Sz�nek be�ll�t�sa
        SpriteRenderer renderer = bulletLeft.GetComponent<SpriteRenderer>();
        renderer.color = color;
        renderer = bulletRight.GetComponent<SpriteRenderer>();
        renderer.color = color;
    }
    public void FollowPlayer()
    {
        if (playerTransform != null)
        {
            // Az aktu�lis objektum X poz�ci�j�t folyamatosan a player X poz�ci�j�hoz igaz�tjuk
            Vector3 newPosition = new Vector3(playerTransform.position.x, 3, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
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
            playerScript.AddToScore(20);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
        }

    }
}
