using UnityEngine;

public class MoveAsteroid : MonoBehaviour
{
    public float speed = 5;
    public float deadZone =-7;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
}
