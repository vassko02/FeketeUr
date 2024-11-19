using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    public float speed=5f;
    public float rotateSpeed = 200f;
    private Rigidbody2D rb;
    private Player playerScript;
    private GameObject playerGO;
    public GameObject explosion;

    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        if (playerGO != null)
        {
            playerScript = playerGO.GetComponent<Player>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerGO != null) 
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag!="Orion")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            if (collision.gameObject.tag == "Player")
            {
                playerScript.TakeDamage(35);
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Asteroid")
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "PlayerProjectile")
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

    }
}
