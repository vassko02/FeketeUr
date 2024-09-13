using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Lövedék sebessége
    public float deadZone = 7; // Az a zóna ahol a lövedéket töröljük

    void Update()
    {
        // A lövedék mozgatása
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // A lövedék törlése, ha a képernyõn kívülre kerül
        if (transform.position.y > deadZone)
        {
            Destroy(gameObject);
        }
    }
}
