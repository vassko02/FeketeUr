using UnityEngine;

public class MovePlanet : MonoBehaviour
{
    public float speed = 1;
    public float deadZone = -10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.down * speed) * Time.deltaTime;
        Debug.Log(deadZone);

        if (transform.position.y < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
