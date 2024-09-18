using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Lövedék sebessége
    public float deadZone = 7; // Az a zóna, ahol a lövedéket töröljük
    public Vector3 direction = Vector3.right; // Az irány, amerre a lövedék mozog

    void Update()
    {
        // A lövedék mozgatása az irány szerint
        transform.Translate(direction * speed * Time.deltaTime);

        // A lövedék törlése, ha a képernyőn kívülre kerül
        if (deadZone>0)
        {
            if (transform.position.y > deadZone)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.y < deadZone)
            {
                Destroy(gameObject);
            }
        }
    }
}