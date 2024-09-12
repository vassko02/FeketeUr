using UnityEngine;

public class MoveAsteroid : MonoBehaviour
{
    public float speed = 5;
    public float deadZone =-7;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));
        deadZone = screenBottom.y;
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
}
