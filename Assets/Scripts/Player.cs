using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public TMP_Text countdownText; // A UI Text elem, ami a visszasz�ml�l�t mutatja
    private float buffTimer; // Visszasz�ml�l� id�z�t�

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
    public GameObject DamageBuffUI;

    private bool hasDamageBuff=false;
    private float buffDuration = 10f; // Mennyi ideig tart a buff
    private int normalBulletDamage = 50; // Norm�l l�ved�k sebz�se
    private int buffedBulletDamage = 100; // Buffolt l�ved�k sebz�se
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

        buffTimer = buffDuration;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0);
        transform.position += move * moveSpeed * Time.deltaTime;

        // A j�t�kos k�perny�n bel�l tart�sa
        keepPlayerInBounds();
        if (hasDamageBuff)
        {
            // Cs�kkentj�k az id�z�t�t
            buffTimer -= Time.deltaTime;

            // Friss�tj�k a visszasz�ml�l�t a UI-on
            countdownText.text = Mathf.Round(buffTimer).ToString();
        }
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
            int bulletDamage = hasDamageBuff ? buffedBulletDamage : normalBulletDamage; // A sebz�s meghat�roz�sa

            GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));

            Bullet bulletScriptL = bulletLeft.GetComponent<Bullet>();
            if (bulletScriptL != null)
            {
                bulletScriptL.SetDamage(bulletDamage); // Sebz�s be�ll�t�sa a l�ved�ken
            }
            Bullet bulletScriptR = bulletRight.GetComponent<Bullet>();
            if (bulletScriptR != null)
            {
                bulletScriptR.SetDamage(bulletDamage); // Sebz�s be�ll�t�sa a l�ved�ken
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
            DamageBuffUI.SetActive(false);

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
            ActivateDamageBuff();
            Destroy(collision.gameObject);
        }

    }
    void ActivateDamageBuff()
    {
        hasDamageBuff = true; // Buff aktiv�lva
        DamageBuffUI.SetActive(true);
        StartCoroutine(BuffTimer());
    }
    private void EndBuff()
    {
        hasDamageBuff = false; // Buff kikapcsol�sa
        DamageBuffUI.SetActive(false);
        buffTimer = buffDuration;

    }
    IEnumerator BuffTimer()
    {
        yield return new WaitForSeconds(buffDuration); // V�rakoz�s a buff idej�ig
        EndBuff();
    }
}