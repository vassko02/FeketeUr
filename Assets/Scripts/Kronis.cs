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

    public float moveSpeed = 3f; // Mozg�s sebess�ge
    private Vector3 rightPosition; // C�lpoz�ci� a jobb oldalon
    private Vector3 leftPosition;  // C�lpoz�ci� a bal oldalon
    private Vector3 targetPosition; // Az aktu�lis c�lpoz�ci�

    private int currentStage = 1;

    public GameObject explosion;
    public GameObject target;
    public float delayBeforeAttack = 2f;   // Mennyi id� m�lva t�rt�njen a t�mad�s a jel�l�s ut�n
    public float markerDuration = 1f;      // Mennyi ideig maradjon a jel�l� l�that�

    public float attackInterval = 1.0f; // T�mad�si intervallum (m�sodperc)
    private float attackTimer = 0f; // Id�z�t� a t�mad�shoz

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

        rightPosition = new Vector3(10f, 3.8f, transform.position.z); // Jobb sz�l
        leftPosition = new Vector3(-10f, 3.8f, transform.position.z);  // Bal sz�l
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
        // Friss�tsd az id�z�t�t
        attackTimer += Time.deltaTime;

        // Ha el�rte az intervallumot, t�madj
        if (attackTimer >= attackInterval&&player!=null)
        {
            Attack();
            attackTimer = 0f; // Reseteld az id�z�t�t
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Ellen�rizd, ha az ellens�g el�rte a c�lpoz�ci�t
        if (transform.position == targetPosition)
        {
            // Ha a c�l a bal oldal volt, �ll�tsuk a c�lt a jobb oldalra, �s ford�tva
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
        // J�t�kos aktu�lis poz�ci�j�nak lek�r�se
        if (playerTransform!=null)
        {
            Vector3 targetPosition = playerTransform.position;

            // Jel�l� l�trehoz�sa a j�t�kos poz�ci�j�n
            GameObject marker = Instantiate(target, targetPosition, Quaternion.identity);

            // Jel�l� elt�nik egy id� ut�n, majd t�mad�s k�vetkezik
            StartCoroutine(ExecuteAttack(marker, targetPosition));
        }
    }

    private IEnumerator ExecuteAttack(GameObject marker, Vector3 targetPosition)
    {
        // Jel�l� l�that�s�g�nak id�z�t�se
        yield return new WaitForSeconds(markerDuration);

        // Jel�l� elt�ntet�se
        Destroy(marker);

        // V�rakoz�s, miel�tt a t�mad�s megt�rt�nik

        GameObject explosionInstance = Instantiate(explosion, targetPosition, Quaternion.identity);

        // J�t�kos poz�ci�j�nak lek�r�se
        Vector3 playerPosition = playerTransform.position; // Felt�telezve, hogy van playerTransform

        // Sz�mold ki a t�vols�got
        float distanceToPlayer = Vector3.Distance(targetPosition, playerPosition);

        // Ellen�rizz�k, hogy a j�t�kos a robban�s hat�k�r�n bel�l van-e
        if (distanceToPlayer <= 2f)
        {
            // T�mad�s a j�t�kosra, p�ld�ul sebz�s okoz�sa
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
