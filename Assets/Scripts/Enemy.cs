using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabRight; // A lövedék prefabja
    public GameObject bulletPrefabLeft; // A lövedék prefabja
    public float moveSpeed = 5f; // Player mozgási sebessége
    public float fireRate = 0.5f; // Lövések közötti idõköz

    private float nextFireTime = 0f;

    private Transform playerTransform; // A player pozíciójának tárolására
    private GameObject player;
    private Vector3 targetPosition;

    public int maxHealt = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealt;
        // A hajó méretének kiszámítása
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

        // Az ellenség lövése
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
        // Lövés
        void Shoot()
        {
            // Lövedékek létrehozása
            GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + 0.50f, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0, 0, -90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - 0.50f, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(0, 0, -90));
        }
        void FollowPlayer()
        {
            if (playerTransform != null)
            {
                // Az aktuális objektum X pozícióját folyamatosan a player X pozíciójához igazítjuk
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