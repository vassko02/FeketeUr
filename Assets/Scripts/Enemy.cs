using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabRight; // A l�ved�k prefabja
    public GameObject bulletPrefabLeft; // A l�ved�k prefabja
    public float moveSpeed = 5f; // Player mozg�si sebess�ge
    public float fireRate = 0.5f; // L�v�sek k�z�tti id�k�z

    private float nextFireTime = 0f;

    private Transform playerTransform; // A player poz�ci�j�nak t�rol�s�ra
    private GameObject player;
    private Vector3 targetPosition;

    public int maxHealt = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealt;
        // A haj� m�ret�nek kisz�m�t�sa
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {

        FollowPlayer();

        // Az ellens�g l�v�se
        if (Time.time > nextFireTime)
        {
            if (player!=null)
            {
                if (player.transform.position.x == transform.position.x)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;

                }
            }
        }
        // L�v�s
        void Shoot()
        {
            // L�ved�kek l�trehoz�sa
            GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + 0.50f, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0, 0, -90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - 0.50f, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0, 0, -90));
        }
        void FollowPlayer()
        {
            if (playerTransform != null)
            {
                // Az aktu�lis objektum X poz�ci�j�t folyamatosan a player X poz�ci�j�hoz igaz�tjuk
                Vector3 newPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            }
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
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(50);
        }
    }
}