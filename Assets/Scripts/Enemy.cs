using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabRight; // A lövedék prefabja
    public GameObject bulletPrefabLeft; // A lövedék prefabja
    public float moveSpeed = 5f; // Player mozgási sebessége
    public float fireRate = 0.5f; // Lövések közötti idõköz
    public float shootRange = 2f;
    private float nextFireTime = 0f;

    private Transform playerTransform; // A player pozíciójának tárolására
    private GameObject player;
    private Vector3 targetPosition;

    public int maxHealt = 100;
    public int currentHealth;

    public float separationRadius = 1.5f; // Minimális távolság az ellenségek között
    public LayerMask enemyLayer;
    private Player playerScript;
    public Color color;
    private float cooldownTimer = 0f; // Lövési idõzítõ


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
                // Ellenõrizzük, hogy a játékos X pozíciója közel van-e az ellenség X pozíciójához
                if (Mathf.Abs(player.transform.position.x - transform.position.x) <= shootRange)
                {
                    Shoot();
                    cooldownTimer = 0f; // Számláló újraindítása
                }
            }
        }
    }

    // Lövés
    public void Shoot()
    {
        // Lövedékek létrehozása
        float offset = 0.5f;

        GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + offset, transform.position.y - offset, transform.position.z), Quaternion.Euler(0, 0, -90));
        GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - offset, transform.position.y - offset, transform.position.z), Quaternion.Euler(0, 0, -90));

        // Színek beállítása
        SpriteRenderer renderer = bulletLeft.GetComponent<SpriteRenderer>();
        renderer.color = color;
        renderer = bulletRight.GetComponent<SpriteRenderer>();
        renderer.color = color;
    }
    public void FollowPlayer()
    {
        if (playerTransform != null)
        {
            // Az aktuális objektum X pozícióját folyamatosan a player X pozíciójához igazítjuk
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
