using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabRight; // A l�ved�k prefabja
    public GameObject bulletPrefabLeft; // A l�ved�k prefabja
    public GameObject Player;
    public Transform firePointRight; // A jobb oldali l�ved�k ind�t�si pontja
    public Transform firePointLeft; // A bal oldali l�ved�k ind�t�si pontja
    public float moveSpeed = 5f; // Player mozg�si sebess�ge
    public float fireRate = 0.5f; // L�v�sek k�z�tti id�k�z


    private float nextFireTime = 0f;

    private Vector3 targetPosition;
    public int maxHealt = 100;
    public int currentHealth;
    void Start()
    {
        // A haj� m�ret�nek kisz�m�t�sa
        currentHealth = maxHealt;
        
    }

    void Update()
    {
        FollowPlayer();

        // Az ellens�g l�v�se
        if (Time.time > nextFireTime)
        {
            if (Player.transform.position.x == transform.position.x) {
                Shoot();
                nextFireTime = Time.time + fireRate;

            }
        }
        // L�v�s
        void Shoot()
        {
            // L�ved�kek l�trehoz�sa
            GameObject bulletRight = Instantiate(bulletPrefabRight, firePointRight.position, firePointRight.rotation * Quaternion.Euler(0, 0, -90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, firePointLeft.position, firePointLeft.rotation * Quaternion.Euler(0, 0, -90));
        }
        void FollowPlayer()
        {
            // Csak az X tengely ment�n mozgatjuk az ellens�get a j�t�kos fel�
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