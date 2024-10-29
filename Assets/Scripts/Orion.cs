using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Orion : MonoBehaviour
{
    public int maxHealth = 800;
    public int currentHealth;
    public HealthBar healthBar;

    private Player playerScript;
    private GameObject player;
    private Transform playerTransform;
    public float speed = 5f; // Ráhajtási sebesség
    public float rushSpeed = 10f; // Ráhajtási sebesség

    private float attackTimer = 0f; // Idõzítõ a támadáshoz
    public float attackInterval = 3f; // Támadási intervallum (másodperc)
    public float waitTimeBeforeRush = 1.5f; // Várakozási idõ a ráhajtás elõtt

    private float shootTimer = 0f;
    public float shootInterval = 2f;


    private GameObject progressManager;
    private ProgressManager progressManagerScript;

    private Quaternion originalRotation; // A boss eredeti rotációja
    private Vector3 originalPosition; // A boss eredeti pozíciója
    private bool isAttacking = false; // Egy támadás folyamatban van-e

    public float rocketTimeSpan = 10f;

    public GameObject Missile;
    public GameObject Target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        originalPosition = new Vector3(0, 0, 0);
        originalRotation = transform.rotation;

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
    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        // Ha elérte az intervallumot, támadj
        if (attackTimer >= attackInterval && !isAttacking)
        {
            StartCoroutine(Attack());
            attackTimer = 0f; // Reseteld az idõzítõt
        }
        shootTimer += Time.deltaTime;

        // Ha elérte vagy meghaladta a beállított intervallumot, lövés
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f; // Az idõzítõ visszaállítása
        }
    }
    private void Shoot()
    {
        Vector3 spawnPosition = transform.position;

        // Generálj egy véletlenszerû irányt
        float randomAngle = UnityEngine.Random.Range(0f, 360f); // Véletlenszerû szög (0-360 fok)
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle); // Forgatás Quaternion formában

        GameObject rocket = Instantiate(Missile, spawnPosition, randomRotation);
    }

    private IEnumerator Attack()
    {
        isAttacking = true; // Megjelöljük, hogy támadás folyamatban van
        // Játékos aktuális pozíciójának lekérése
        if (playerTransform != null)
        {
            Vector3 targetPosition = playerTransform.position;
             GameObject targetInstance= Instantiate(Target, targetPosition, Quaternion.identity);

            // 1. A boss ráfordul a játékosra
            Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
            transform.rotation = lookRotation;

            // 2. Várakozás 1.5 másodpercig
            yield return new WaitForSeconds(waitTimeBeforeRush);

            // 3. Ráhajtás a játékosra
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, rushSpeed * Time.deltaTime);
                yield return null;
            }
            Destroy(targetInstance);
            // 4. Visszatérés az eredeti pozícióra
            while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
                yield return null;
            }
            transform.rotation = originalRotation;

        }

        isAttacking = false; // Támadás vége
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

            playerScript.AddToScore(1000);
            playerScript.moveSpeed = 5f;
            playerScript.bulletSpeed = 10f;

            healthBar.gameObject.SetActive(false);
            progressManagerScript.doneWithDialog = false;
            progressManagerScript.midBossFight = false;
            progressManagerScript.elapsedTime++;

            GameObject[] rockets = GameObject.FindGameObjectsWithTag("Rocket");

            // Destroy the found GameObjects
            foreach (GameObject kronisAttack in rockets)
            {
                Destroy(kronisAttack);
            }
            Destroy(gameObject);
        }
        healthBar.setHealth(currentHealth);
    }

}
