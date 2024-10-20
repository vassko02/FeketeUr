using UnityEngine;

public class MoveAsteroid : MonoBehaviour
{
    private float speed ;
    public float minSpeed = 3;
    public float maxSpeed = 7;
    public float deadZone =-7;
    private Player playerScript;
    public GameObject explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerScript = playerObject.GetComponent<Player>();
        }
        speed = Random.Range(minSpeed, maxSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.down * speed) * Time.deltaTime;
        if (transform.position.y < deadZone)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Asteroid")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);

            if (collision.gameObject.CompareTag("PlayerProjectile"))
            {
                Destroy(collision.gameObject);
                playerScript.AddToScore(10);
                Destroy(gameObject);
            }
        }

    }
}
