using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;


public class Player : MonoBehaviour
{
    public bool newGame;
    public string playerName;
    private Vector3 playerPosition; // J�t�kos poz�ci�ja

    public Text countdownText; // A UI Text elem, ami a visszasz�ml�l�t mutatja
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
    public GameObject BuffUI;

    private bool hasShiledBuff = false;
    private bool hasDamageBuff = false;
    private float buffDuration = 5f; // Mennyi ideig tart a buff
    private int normalBulletDamage = 50; // Norm�l l�ved�k sebz�se
    private int buffedBulletDamage = 100; // Buffolt l�ved�k sebz�se

    public float bulletSpeed =10f;

    public GameObject shieldBuffUIImage; // Az Image komponens referencia
    public GameObject damageBuffUIImage; // Az Image komponens referencia

    public ProgressManager progressManager;

    public int score = 0;
    public float scoreIncreaseRate = 0.5f; // Milyen gyakran n�vekszik a score (m�sodpercben)
    public int scoreIncrement = 5; // Mennyivel n�vekszik a score
    private float nextScoreIncreaseTime = 0f;

    public Text scoreText; // UI Text, ami megjelen�ti a score-t
    public Text buffPicupText;
    void Start()
    {
        MenuMusic backgroundMusic = FindObjectOfType<MenuMusic>();
        if (backgroundMusic != null)
        {
            backgroundMusic.GetComponent<AudioSource>().Pause();
        }
        SaveManager.Instance.Load();
        if (PlayerPrefs.GetInt("continue")==1)
        {
            LoadGame();
        }


        // A k�perny� sz�leinek kisz�m�t�sa
        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        // A haj� m�ret�nek kisz�m�t�sa
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        audioSource = GetComponent<AudioSource>();

        buffTimer = buffDuration;

        UpdateScoreUI();
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
        if (hasShiledBuff)
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


        if (Time.time >= nextScoreIncreaseTime&&progressManager.gameObject.GetComponent<ProgressManager>().midBossFight==false && progressManager.gameObject.GetComponent<ProgressManager>().midDialog == false)
        {
            score += scoreIncrement;
            nextScoreIncreaseTime = Time.time + scoreIncreaseRate;
            UpdateScoreUI();
        }

    }
    public void keepPlayerInBounds()
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
    public void Shoot()
    {
        // L�ved�kek l�trehoz�sa
        float offset = 0.4f;
        int bulletDamage;
        Color bulletColor;
        if (hasDamageBuff)
        {
            bulletDamage = buffedBulletDamage;
            bulletColor = Color.yellow;
        }
        else
        {
            bulletDamage = normalBulletDamage;
            bulletColor = Color.white;
        }
        GameObject bulletRight = Instantiate(bulletPrefabRight, new Vector3(transform.position.x + offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));
        GameObject bulletLeft = Instantiate(bulletPrefabLeft, new Vector3(transform.position.x - offset, transform.position.y + offset, transform.position.z), Quaternion.Euler(0, 0, 90));

        Bullet bulletScriptL = bulletLeft.GetComponent<Bullet>();
        SpriteRenderer renderer = bulletLeft.GetComponent<SpriteRenderer>();
        if (bulletScriptL != null)
        {
            bulletScriptL.SetDamage(bulletDamage); // Sebz�s be�ll�t�sa a l�ved�ken
            bulletScriptL.speed = bulletSpeed;
            renderer.color = bulletColor;
        }
        Bullet bulletScriptR = bulletRight.GetComponent<Bullet>();
        renderer = bulletRight.GetComponent<SpriteRenderer>();

        if (bulletScriptR != null)
        {
            bulletScriptR.SetDamage(bulletDamage); // Sebz�s be�ll�t�sa a l�ved�ken
            bulletScriptR.speed = bulletSpeed;
            renderer.color = bulletColor;

        }
    }
    public void TakeDamage(int damage)
    {
        if (hasShiledBuff==false)
        {
            if (currentHealth - damage > 0)
            {
                currentHealth -= damage;
            }
            else
            {
                currentHealth -= damage;

                GameOver();
            }
            if (healthBar != null)
            {
                healthBar.setHealth(currentHealth);
            }
        }

    }
    public void GameOver()
    {
        Time.timeScale = 0f;

        BuffUI.SetActive(false);
        healthBar.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        Destroy(gameObject);
        gameOverScreen.SetActive(true);
        SaveManager.Instance.saveData.currentRunData = null;
        SaveManager.Instance.Save();

    }
    public void AddToScore(int amount)
    {
        score += amount;
        if (scoreText !=null)
        {
            UpdateScoreUI();
        }
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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            TakeDamage(20);
            AddToScore(10);
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
            AddToScore(20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Kronis"))
        {
            TakeDamage(50);
        }
        if (collision.gameObject.CompareTag("Orion"))
        {
            TakeDamage(50);
        }
        if (collision.gameObject.CompareTag("Nyx"))
        {
            TakeDamage(50);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        buffPicupText.gameObject.SetActive(true);
        if (collision.gameObject.CompareTag("HealBuff"))
        {
            buffPicupText.text = "Healed for 30 HP!";
            Heal(30);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("DamageBuff"))
        {
            ActivateDamageBuff();
            buffPicupText.text = "Damage increased for 5s!";
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("ShieldBuff"))
        {
            ActivateShieldBuff();
            buffPicupText.text = "Shield activated for 5s!";
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("MaxHealthBuff"))
        {
            buffPicupText.text = "Maximum health increased!";
            maxHealt += 20;
            healthBar.SetMaxHealth(maxHealt, true);
            Heal(20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("ScoreBuff"))
        {
            buffPicupText.text = "Score multiplier increased!";
            scoreIncrement += 2;
            Destroy(collision.gameObject);
        }
        StartCoroutine(ToggleBuffPicup(2f));
    }

    private IEnumerator ToggleBuffPicup(float v)
    {
        yield return new WaitForSeconds(v);
        buffPicupText.gameObject.SetActive(false);
    }

    public void ActivateDamageBuff()
    {
        hasDamageBuff = true; // Buff aktiv�lva
        shieldBuffUIImage.SetActive(false);
        damageBuffUIImage.SetActive(true);
        BuffUI.SetActive(true);
        StartCoroutine(BuffTimer());
    }
    public void ActivateShieldBuff()
    {
        hasShiledBuff = true; // Buff aktiv�lva
        shieldBuffUIImage.SetActive(true);
        damageBuffUIImage.SetActive(false);

        BuffUI.SetActive(true);
        StartCoroutine(BuffTimer());
    }
    private void EndBuff()
    {
        hasShiledBuff = false; // Buff kikapcsol�sa
        hasDamageBuff = false; // Buff kikapcsol�sa
        BuffUI.SetActive(false);
        buffTimer = buffDuration;

    }
    IEnumerator BuffTimer()
    {
        yield return new WaitForSeconds(buffDuration); // V�rakoz�s a buff idej�ig
        EndBuff();
    }

    public void SaveGame()
    {
        //SaveManager.Instance.saveData.currentRunData.PlayerName = playerName;
        SaveManager.Instance.saveData.currentRunData.CurrentHealth = currentHealth;
        SaveManager.Instance.saveData.currentRunData.MaxHealth = maxHealt;
        SaveManager.Instance.saveData.currentRunData.ScoreIncrement = scoreIncrement;
        SaveManager.Instance.saveData.currentRunData.Score = score;
        SaveManager.Instance.saveData.currentRunData.ElapsedTime = progressManager.elapsedTime;
        // Mentj�k a currentRun adatokat
        SaveManager.Instance.Save();
    }
    public void LoadGame()
    {
        SaveManager.Instance.Load();
        if (SaveManager.Instance.saveData.currentRunData != null) // Ellen�rizd, hogy az adatok bet�lt�dtek
        {
            this.currentHealth = SaveManager.Instance.saveData.currentRunData.CurrentHealth;
            this.maxHealt = SaveManager.Instance.saveData.currentRunData.MaxHealth;
            this.score = SaveManager.Instance.saveData.currentRunData.Score;
            this.playerName = SaveManager.Instance.saveData.currentRunData.PlayerName;
            this.scoreIncrement = SaveManager.Instance.saveData.currentRunData.ScoreIncrement;

            healthBar.SetMaxHealth(maxHealt, true);
            healthBar.setHealth(currentHealth);

        }
        else
        {
            Debug.LogError("Failed to load CurrentRun data.");
        }

    }
}
