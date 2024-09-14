using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject bulletPrefabRight; // A lövedék prefabja
    public GameObject bulletPrefabLeft; // A lövedék prefabja
    public Transform firePointRight; // A jobb oldali lövedék indítási pontja
    public Transform firePointLeft; // A bal oldali lövedék indítási pontja
    public float moveSpeed = 5f; // Player mozgási sebessége
    public float fireRate = 0.5f; // Lövések közötti idõköz

    private Vector2 screenBounds; // A képernyõ szélei
    private float objectWidth;
    private float objectHeight;
    private float nextFireTime = 0f;
    private AudioSource audioSource; //Lövés hang

    public int maxHealt = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject gameOverScreen;

    void Start()
    {
        // A képernyõ széleinek kiszámítása
        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        // A hajó méretének kiszámítása
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

        // A játékos képernyõn belül tartása
        keepPlayerInBounds();

        // Lövés
        if (Input.GetMouseButtonDown(0)) // Bal egérgomb lenyomása
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Heal(20);
        }
        void keepPlayerInBounds()
        {
            // Játékos aktuális pozíciója
            Vector3 pos = transform.position;

            // Pozíció korlátozása a képernyõn belülre
            pos.x = Mathf.Clamp(pos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
            pos.y = Mathf.Clamp(pos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);

            // Pozíció firssítése
            transform.position = pos;
        }

        // Lövés
        void Shoot()
        {
            // Lövedékek létrehozása
            GameObject bulletRight = Instantiate(bulletPrefabRight, firePointRight.position, firePointRight.rotation * Quaternion.Euler(0, 0, 90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, firePointLeft.position, firePointLeft.rotation * Quaternion.Euler(0, 0, 90));

            //Hang egyszeri lejátszása, ha gyorsan lõ egymás után akkor sem rétegezõdnek a hangok
            audioSource.PlayOneShot(audioSource.clip);
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
            Destroy(collision.gameObject); // Az aszteroida eltávolítása

        }
    }
}