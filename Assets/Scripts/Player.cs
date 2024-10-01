using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Player : MonoBehaviour
{
    public bool newGame;
    public string playerName;
    private Vector3 playerPosition; // Játékos pozíciója
    public string saveFilePath;


    public TMP_Text countdownText; // A UI Text elem, ami a visszaszámlálót mutatja
    private float buffTimer; // Visszaszámláló idõzítõ

    public GameObject bulletPrefabRight; // A lövedék prefabja
    public GameObject bulletPrefabLeft; // A lövedék prefabja
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
    public GameObject BuffUI;

    private bool hasShiledBuff = false;
    private bool hasDamageBuff = false;
    private float buffDuration = 10f; // Mennyi ideig tart a buff
    private int normalBulletDamage = 50; // Normál lövedék sebzése
    private int buffedBulletDamage = 100; // Buffolt lövedék sebzése


    public GameObject shieldBuffUIImage; // Az Image komponens referencia
    public GameObject damageBuffUIImage; // Az Image komponens referencia


    public int score = 0;
    public float scoreIncreaseRate = 0.5f; // Milyen gyakran növekszik a score (másodpercben)
    public int scoreIncrement = 5; // Mennyivel növekszik a score
    private float nextScoreIncreaseTime = 0f;

    public TMP_Text scoreText; // UI Text, ami megjeleníti a score-t


    void Start()
    {
        if (!newGame)
        {
            LoadGame();
        }
        // A képernyõ széleinek kiszámítása
        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        // A hajó méretének kiszámítása
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealt;
        healthBar.SetMaxHealth(maxHealt, false);

        buffTimer = buffDuration;

        UpdateScoreUI();
        saveFilePath = Application.persistentDataPath+"savefile.json";

        InvokeRepeating("SaveGame", 10f, 10f);
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0);
        transform.position += move * moveSpeed * Time.deltaTime;

        // A játékos képernyõn belül tartása
        keepPlayerInBounds();
        if (hasDamageBuff)
        {
            // Csökkentjük az idõzítõt
            buffTimer -= Time.deltaTime;

            // Frissítjük a visszaszámlálót a UI-on
            countdownText.text = Mathf.Round(buffTimer).ToString();
        }
        if (hasShiledBuff)
        {
            // Csökkentjük az idõzítõt
            buffTimer -= Time.deltaTime;

            // Frissítjük a visszaszámlálót a UI-on
            countdownText.text = Mathf.Round(buffTimer).ToString();
        }
        // Lövés
        if (Input.GetMouseButtonDown(0)) // Bal egérgomb lenyomása
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
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
            float offset = 0.4f;
            int bulletDamage = hasDamageBuff ? buffedBulletDamage : normalBulletDamage; // A sebzés meghatározása

            GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));
            GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));

            Bullet bulletScriptL = bulletLeft.GetComponent<Bullet>();
            if (bulletScriptL != null)
            {
                bulletScriptL.SetDamage(bulletDamage); // Sebzés beállítása a lövedéken
            }
            Bullet bulletScriptR = bulletRight.GetComponent<Bullet>();
            if (bulletScriptR != null)
            {
                bulletScriptR.SetDamage(bulletDamage); // Sebzés beállítása a lövedéken
            }
        }

        if (Time.time >= nextScoreIncreaseTime)
        {
            score += scoreIncrement;
            nextScoreIncreaseTime = Time.time + scoreIncreaseRate;
            UpdateScoreUI();
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
            BuffUI.SetActive(false);

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

    public void AddToScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
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
            if (hasShiledBuff == false)
            {
                TakeDamage(20);
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            if (hasShiledBuff == false)
            {
                TakeDamage(25);
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hasShiledBuff == false)
            {
                TakeDamage(35);
            }
            Destroy(collision.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        if (collision.gameObject.CompareTag("ShieldBuff"))
        {
            ActivateShieldBuff();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("MaxHealthBuff"))
        {
            maxHealt += 20;
            healthBar.SetMaxHealth(maxHealt, true);
            Destroy(collision.gameObject);
        }
    }
    void ActivateDamageBuff()
    {
        hasDamageBuff = true; // Buff aktiválva
        shieldBuffUIImage.SetActive(false);
        damageBuffUIImage.SetActive(true);
        BuffUI.SetActive(true);
        StartCoroutine(BuffTimer());
    }
    void ActivateShieldBuff()
    {
        hasShiledBuff = true; // Buff aktiválva
        shieldBuffUIImage.SetActive(true);
        damageBuffUIImage.SetActive(false);

        BuffUI.SetActive(true);
        StartCoroutine(BuffTimer());
    }
    private void EndBuff()
    {
        hasShiledBuff = false; // Buff kikapcsolása
        hasDamageBuff = false; // Buff kikapcsolása
        BuffUI.SetActive(false);
        buffTimer = buffDuration;

    }
    IEnumerator BuffTimer()
    {
        yield return new WaitForSeconds(buffDuration); // Várakozás a buff idejéig
        EndBuff();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerName = this.playerName,
            score = this.score,
            maxHealth = this.maxHealt,
            currentHealth = this.currentHealth,
            playerPosition = transform.position
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to " +saveFilePath);
    }
    public void LoadGame()
    {
        if (string.IsNullOrEmpty(saveFilePath))
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            this.playerName = loadedData.playerName;
            this.score = loadedData.score;
            this.maxHealt = loadedData.maxHealth;
            this.currentHealth = loadedData.currentHealth;
            transform.position = loadedData.playerPosition;

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }
}