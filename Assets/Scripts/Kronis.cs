using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Kronis : MonoBehaviour
{
    public int maxHealth=800;
    public int currentHealth;
    public HealthBar healthBar;

    private Player playerScript;
    private GameObject player;
    private Transform playerTransform;

    public float moveSpeed = 3f; // Mozgás sebessége
    private Vector3 rightPosition; // Célpozíció a jobb oldalon
    private Vector3 leftPosition;  // Célpozíció a bal oldalon
    private Vector3 targetPosition; // Az aktuális célpozíció

    private int currentStage = 1;

    public GameObject explosion;
    public GameObject target;
    public float delayBeforeAttack = 2f;   // Mennyi idõ múlva történjen a támadás a jelölés után
    public float markerDuration = 1f;      // Mennyi ideig maradjon a jelölõ látható

    public float attackInterval = 1.0f; // Támadási intervallum (másodperc)
    private float attackTimer = 0f; // Idõzítõ a támadáshoz

    private GameObject progressManager;
    private ProgressManager progressManagerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
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

        rightPosition = new Vector3(10f, 3.8f, transform.position.z); // Jobb szél
        leftPosition = new Vector3(-10f, 3.8f, transform.position.z);  // Bal szél
        targetPosition = leftPosition;
        BoostPlayerSpeed();
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Move();
        }
        // Frissítsd az idõzítõt
        attackTimer += Time.deltaTime;

        // Ha elérte az intervallumot, támadj
        if (attackTimer >= attackInterval&&player!=null)
        {
            Attack();
            attackTimer = 0f; // Reseteld az idõzítõt
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Ellenõrizd, ha az ellenség elérte a célpozíciót
        if (transform.position == targetPosition)
        {
            // Ha a cél a bal oldal volt, állítsuk a célt a jobb oldalra, és fordítva
            if (targetPosition == leftPosition)
            {
                targetPosition = rightPosition;
            }
            else
            {
                targetPosition = leftPosition;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            CheckStage();
            Attack();
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
            GameObject[] kronisAttacks = GameObject.FindGameObjectsWithTag("KronisAttack");

            // Destroy the found GameObjects
            foreach (GameObject kronisAttack in kronisAttacks)
            {
                Destroy(kronisAttack);
            }
            Destroy(gameObject);
            
        }
        healthBar.setHealth(currentHealth);
    }

    public void Attack()
    {
        // Játékos aktuális pozíciójának lekérése
        if (playerTransform!=null)
        {
            Vector3 targetPosition = playerTransform.position;

            // Jelölõ létrehozása a játékos pozícióján
            GameObject marker = Instantiate(target, targetPosition, Quaternion.identity);

            // Jelölõ eltûnik egy idõ után, majd támadás következik
            StartCoroutine(ExecuteAttack(marker, targetPosition));
        }
    }

    private IEnumerator ExecuteAttack(GameObject marker, Vector3 targetPosition)
    {
        // Jelölõ láthatóságának idõzítése
        yield return new WaitForSeconds(markerDuration);

        // Jelölõ eltûntetése
        Destroy(marker);

        // Várakozás, mielõtt a támadás megtörténik

        GameObject explosionInstance = Instantiate(explosion, targetPosition, Quaternion.identity);

        // Játékos pozíciójának lekérése
        Vector3 playerPosition = playerTransform.position; // Feltételezve, hogy van playerTransform

        // Számold ki a távolságot
        float distanceToPlayer = Vector3.Distance(targetPosition, playerPosition);

        // Ellenõrizzük, hogy a játékos a robbanás hatókörén belül van-e
        if (distanceToPlayer <= 2f)
        {
            // Támadás a játékosra, például sebzés okozása
            playerScript.TakeDamage(30);
        }
        yield return new WaitForSeconds(3f);
        Destroy(explosionInstance);

    }
    void CheckStage()
    {
        if (currentHealth <= maxHealth * 0.25f && currentStage < 4)
        {
            currentStage++;

            SlowPLayerSpeed();
        }
        else if (currentHealth <= maxHealth * 0.5f && currentStage < 3)
        {
            currentStage++;

            BoostPlayerSpeed();
        }
        else if (currentHealth <= maxHealth * 0.75f && currentStage < 2)
        {
            currentStage++;

            SlowPLayerSpeed();
        }

    }
    void BoostPlayerSpeed()
    {
        if (player!=null)
        {
            playerScript.moveSpeed = 8f;
            playerScript.bulletSpeed = 8f;
            moveSpeed = 6f;
        }

    }
    void SlowPLayerSpeed()
    {
        if (player != null)
        {
            playerScript.moveSpeed = 3f;
            playerScript.bulletSpeed = 3f;
            moveSpeed = 7f;
        }

    }
}
