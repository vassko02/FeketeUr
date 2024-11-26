using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

using Random = UnityEngine.Random;

public class Nyx : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;

    private GameObject progressManager;
    private ProgressManager progressManagerScript;

    private Player playerScript;
    private GameObject player;
    private Transform playerTransform;

    public GameObject overlay;
    private Vector3 originalScale=new Vector3(10,10,0);

    public GameObject cursedFieldObject;  // A t�glalap GameObject-je

    public float minWidth = 3f;  // T�glalap minim�lis sz�less�ge
    public float maxWidth = 10f; // T�glalap maxim�lis sz�less�ge
    public float minHeight = 3f; // T�glalap minim�lis magass�ga
    public float maxHeight = 10f; // T�glalap maxim�lis magass�ga

    public int damage = 20;   // Sebz�s, ha a j�t�kos bel�l van
    public float delayBeforeDamage = 2f;  // K�sleltet�s m�sodpercben

    private Vector3 cursedFieldSize;  // A t�glalap m�rete
    private Vector3 cursedFieldPosition; // A t�glalap poz�ci�ja

    private float attackTimer;
    public float attackInterval;
    private bool isAttacking;

    public float moveSpeed = 3f; // Mozg�s sebess�ge
    private Vector3 rightPosition; // C�lpoz�ci� a jobb oldalon
    private Vector3 leftPosition;  // C�lpoz�ci� a bal oldalon
    private Vector3 targetPosition; // Az aktu�lis c�lpoz�ci�

    private GameObject field;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
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

        rightPosition = new Vector3(10f, 3.8f, transform.position.z); // Jobb sz�l
        leftPosition = new Vector3(-10f, 3.8f, transform.position.z);  // Bal sz�l
        targetPosition = leftPosition;

        StartCursedField();

    }
    
    public void Update() 
    {
        if (player!=null)
        {
            Move();
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval && isAttacking == false&&player!=null) 
        {
            isAttacking = true;
            StartCursedField();
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
    public IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
    public void StartCursedField()
    {
        // Gener�ljuk a random m�ret� t�glalapot a j�t�kos k�r�l
        GenerateCursedField();
        // �ll�tsuk be a t�glalap GameObject megjelen�s�t
        UpdateCursedFieldVisual();
        // V�rakozunk x m�sodpercet, majd ellen�rizz�k a j�t�kost
        StartCoroutine(CheckPlayerPositionAfterDelay());
    }
    void GenerateCursedField()
    {
        // V�letlenszer� sz�less�g �s magass�g
        float width = Random.Range(minWidth, maxWidth);
        float height = Random.Range(minHeight, maxHeight);

        // A j�t�kos poz�ci�j�t haszn�ljuk a t�glalap k�z�ppontjak�nt
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

        // A t�glalap m�ret�nek be�ll�t�sa
        cursedFieldSize = new Vector3(width, height, 1 ); 
        cursedFieldPosition = player.transform.position;  // A j�t�kos poz�ci�ja lesz a t�glalap k�z�ppontja

    }
    void UpdateCursedFieldVisual()
    {
        field = Instantiate(cursedFieldObject);
        field.transform.position = cursedFieldPosition;
        field.transform.localScale = cursedFieldSize;
    }
    IEnumerator CheckPlayerPositionAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDamage);


        if (IsPlayerInsideCursedField())
        {
            // Sebezz�k meg a j�t�kost
            StartCoroutine(ScreenShake(0.2f, 0.3f)); // 0.2 sec hossz�, 0.3 er�ss�g� r�zk�d�s

            ApplyDamage();
        }
        isAttacking = false;
        Destroy(field);
    }
    bool IsPlayerInsideCursedField()
    {
        if (player!=null)
        {
            // Ellen�rizz�k, hogy a j�t�kos poz�ci�ja a t�glalap m�retein bel�l van-e
            Vector3 playerPos = player.transform.position;
            Vector3 fieldPos = field.transform.position;
            Vector3 fieldScale = field.transform.localScale;

            // X �s Y tengely menti ellen�rz�s 2D j�t�k eset�n
            bool isInsideX = Mathf.Abs(playerPos.x - fieldPos.x) < fieldScale.x / 2;
            bool isInsideY = Mathf.Abs(playerPos.y - fieldPos.y) < fieldScale.y / 2;

            return isInsideX && isInsideY;

        }
        return false;
    }
    void ApplyDamage()
    {
        player.GetComponent<Player>().TakeDamage(damage);
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
            Destroy(field);
            overlay.SetActive(false);
            playerScript.AddToScore(1000);

            healthBar.gameObject.SetActive(false);
            progressManagerScript.doneWithDialog = false;
            progressManagerScript.midBossFight = false;
            progressManagerScript.elapsedTime++;

            Destroy(gameObject);
        }
        healthBar.setHealth(currentHealth);
    }

    private void UpdateOverlayScale()
    {
        float healthRatio = (float)currentHealth / maxHealth;

        // �j sk�la be�ll�t�sa az eredeti sk�la �s az �letpontok ar�nya alapj�n
        Vector3 newScale = originalScale * healthRatio;

        // Alkalmazzuk az �j sk�l�t a bossra
        overlay.transform.localScale = newScale;
    }

    // Update is called once per frame

}
