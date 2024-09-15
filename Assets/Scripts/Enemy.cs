using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabRight; // A lövedék prefabja
    public GameObject bulletPrefabLeft; // A lövedék prefabja
    public GameObject Player;
    public Transform firePointRight; // A jobb oldali lövedék indítási pontja
    public Transform firePointLeft; // A bal oldali lövedék indítási pontja
    public float moveSpeed = 5f; // Player mozgási sebessége
    public float fireRate = 0.5f; // Lövések közötti idõköz


    private float nextFireTime = 0f;

    private Vector3 targetPosition;
    public int maxHealt = 100;
    public int currentHealth;
    void Start()
    {
        // A hajó méretének kiszámítása
        currentHealth = maxHealt;
        
    }

    void Update()
    {
        FollowPlayer();

        // Az ellenség lövése
        if (Time.time > nextFireTime)
        {
            if (Player.transform.position.x == transform.position.x) {
                Shoot();
                nextFireTime = Time.time + fireRate;

            }
        }
        // Lövés
        void Shoot()
        {
            // Lövedékek létrehozása
            GameObject bulletRight = Instantiate(bulletPrefabRight, firePointRight.position, firePointRight.rotation * Quaternion.Euler(0, 0, -90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, firePointLeft.position, firePointLeft.rotation * Quaternion.Euler(0, 0, -90));
        }
        void FollowPlayer()
        {
            // Csak az X tengely mentén mozgatjuk az ellenséget a játékos felé
            if (Player!=null)
            {
                Vector3 targetPosition = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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