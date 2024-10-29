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
    public float speed = 5f; // R�hajt�si sebess�g
    public float rushSpeed = 10f; // R�hajt�si sebess�g

    private float attackTimer = 0f; // Id�z�t� a t�mad�shoz
    public float attackInterval = 3f; // T�mad�si intervallum (m�sodperc)
    public float waitTimeBeforeRush = 1.5f; // V�rakoz�si id� a r�hajt�s el�tt

    private float shootTimer = 0f;
    public float shootInterval = 2f;


    private GameObject progressManager;
    private ProgressManager progressManagerScript;

    private Quaternion originalRotation; // A boss eredeti rot�ci�ja
    private Vector3 originalPosition; // A boss eredeti poz�ci�ja
    private bool isAttacking = false; // Egy t�mad�s folyamatban van-e

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
        // Ha el�rte az intervallumot, t�madj
        if (attackTimer >= attackInterval && !isAttacking)
        {
            StartCoroutine(Attack());
            attackTimer = 0f; // Reseteld az id�z�t�t
        }
        shootTimer += Time.deltaTime;

        // Ha el�rte vagy meghaladta a be�ll�tott intervallumot, l�v�s
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f; // Az id�z�t� vissza�ll�t�sa
        }
    }
    private void Shoot()
    {
        Vector3 spawnPosition = transform.position;

        // Gener�lj egy v�letlenszer� ir�nyt
        float randomAngle = UnityEngine.Random.Range(0f, 360f); // V�letlenszer� sz�g (0-360 fok)
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle); // Forgat�s Quaternion form�ban

        GameObject rocket = Instantiate(Missile, spawnPosition, randomRotation);
    }

    private IEnumerator Attack()
    {
        isAttacking = true; // Megjel�lj�k, hogy t�mad�s folyamatban van
        // J�t�kos aktu�lis poz�ci�j�nak lek�r�se
        if (playerTransform != null)
        {
            Vector3 targetPosition = playerTransform.position;
             GameObject targetInstance= Instantiate(Target, targetPosition, Quaternion.identity);

            // 1. A boss r�fordul a j�t�kosra
            Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
            transform.rotation = lookRotation;

            // 2. V�rakoz�s 1.5 m�sodpercig
            yield return new WaitForSeconds(waitTimeBeforeRush);

            // 3. R�hajt�s a j�t�kosra
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, rushSpeed * Time.deltaTime);
                yield return null;
            }
            Destroy(targetInstance);
            // 4. Visszat�r�s az eredeti poz�ci�ra
            while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
                yield return null;
            }
            transform.rotation = originalRotation;

        }

        isAttacking = false; // T�mad�s v�ge
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
