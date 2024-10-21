using System;
using UnityEngine;

public class Nyx : MonoBehaviour
{
    public int maxHealth = 800;
    public int currentHealth;
    public HealthBar healthBar;

    public float moveSpeed = 3f; // Mozgás sebessége

    private GameObject progressManager;
    private ProgressManager progressManagerScript;


    private Player playerScript;
    private GameObject player;
    private Transform playerTransform;

    public GameObject overlay;
    private Vector3 originalScale=new Vector3(10,10,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        overlay.SetActive(true);

        progressManager = GameObject.FindWithTag("ProgressManager");
        progressManagerScript = progressManager.GetComponent<ProgressManager>();

        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
            playerTransform = player.transform;
        }
        healthBar.gameObject.SetActive(true);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth, false);
    }
    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            UpdateOverlayScale();
        }
        else
        {
            currentHealth -= damage;

            overlay.SetActive(false);
            playerScript.AddToScore(1000);
            playerScript.moveSpeed = 5f;
            playerScript.bulletSpeed = 10f;

            healthBar.gameObject.SetActive(false);

            progressManagerScript.elapsedTime++;

            Destroy(gameObject);
        }
        healthBar.setHealth(currentHealth);
    }

    private void UpdateOverlayScale()
    {
        float healthRatio = (float)currentHealth / maxHealth;

        // Új skála beállítása az eredeti skála és az életpontok aránya alapján
        Vector3 newScale = originalScale * healthRatio;

        // Alkalmazzuk az új skálát a bossra
        overlay.transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
