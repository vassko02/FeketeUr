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

    public GameObject cursedFieldObject;  // A téglalap GameObject-je

    public float minWidth = 3f;  // Téglalap minimális szélessége
    public float maxWidth = 10f; // Téglalap maximális szélessége
    public float minHeight = 3f; // Téglalap minimális magassága
    public float maxHeight = 10f; // Téglalap maximális magassága

    public int damage = 20;   // Sebzés, ha a játékos belül van
    public float delayBeforeDamage = 2f;  // Késleltetés másodpercben

    private Vector3 cursedFieldSize;  // A téglalap mérete
    private Vector3 cursedFieldPosition; // A téglalap pozíciója

    private float attackTimer;
    public float attackInterval;
    private bool isAttacking;

    public float moveSpeed = 3f; // Mozgás sebessége
    private Vector3 rightPosition; // Célpozíció a jobb oldalon
    private Vector3 leftPosition;  // Célpozíció a bal oldalon
    private Vector3 targetPosition; // Az aktuális célpozíció

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

        rightPosition = new Vector3(10f, 3.8f, transform.position.z); // Jobb szél
        leftPosition = new Vector3(-10f, 3.8f, transform.position.z);  // Bal szél
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
        // Generáljuk a random méretû téglalapot a játékos körül
        GenerateCursedField();
        // Állítsuk be a téglalap GameObject megjelenését
        UpdateCursedFieldVisual();
        // Várakozunk x másodpercet, majd ellenõrizzük a játékost
        StartCoroutine(CheckPlayerPositionAfterDelay());
    }
    void GenerateCursedField()
    {
        // Véletlenszerû szélesség és magasság
        float width = Random.Range(minWidth, maxWidth);
        float height = Random.Range(minHeight, maxHeight);

        // A játékos pozícióját használjuk a téglalap középpontjaként
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

        // A téglalap méretének beállítása
        cursedFieldSize = new Vector3(width, height, 1 ); 
        cursedFieldPosition = player.transform.position;  // A játékos pozíciója lesz a téglalap középpontja

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
            // Sebezzük meg a játékost
            StartCoroutine(ScreenShake(0.2f, 0.3f)); // 0.2 sec hosszú, 0.3 erõsségû rázkódás

            ApplyDamage();
        }
        isAttacking = false;
        Destroy(field);
    }
    bool IsPlayerInsideCursedField()
    {
        if (player!=null)
        {
            // Ellenõrizzük, hogy a játékos pozíciója a téglalap méretein belül van-e
            Vector3 playerPos = player.transform.position;
            Vector3 fieldPos = field.transform.position;
            Vector3 fieldScale = field.transform.localScale;

            // X és Y tengely menti ellenõrzés 2D játék esetén
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

        // Új skála beállítása az eredeti skála és az életpontok aránya alapján
        Vector3 newScale = originalScale * healthRatio;

        // Alkalmazzuk az új skálát a bossra
        overlay.transform.localScale = newScale;
    }

    // Update is called once per frame

}
