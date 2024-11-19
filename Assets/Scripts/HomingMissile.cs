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
            // Irány meghatározása a célpont felé
            Vector2 direction = (Vector2)playerGO.transform.position - rb.position;
            direction.Normalize();

            // Fordulás irányának meghatározása
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // Rakéta forgatása
            rb.angularVelocity = -rotateAmount * rotateSpeed;

            // Mozgatás a rakéta elõre mutató irányába (linearVelocity használata)
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
