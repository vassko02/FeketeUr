using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    public float speed=5f;
    public float rotateSpeed = 200f;
    private Rigidbody2D rb;
    private Player playerScript;
    private GameObject playerGO;
    public Transform playerTransfrom;
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerScript = playerGO.GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Ir�ny meghat�roz�sa a c�lpont fel�
        Vector2 direction = (Vector2)playerGO.transform.position - rb.position;
        direction.Normalize();

        // Fordul�s ir�ny�nak meghat�roz�sa
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        // Rak�ta forgat�sa
        rb.angularVelocity = -rotateAmount * rotateSpeed;

        // Mozgat�s a rak�ta el�re mutat� ir�ny�ba (linearVelocity haszn�lata)
        rb.linearVelocity = transform.up * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Player") {
            Destroy(gameObject);
            playerScript.TakeDamage(35);
        }
        else if (collision.gameObject.tag == "Asteroid")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "PlayerProjectile")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
