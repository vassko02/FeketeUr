using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // L�ved�k sebess�ge
    public float deadZone = 7; // Az a z�na ahol a l�ved�ket t�r�lj�k

    void Update()
    {
        // A l�ved�k mozgat�sa
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // A l�ved�k t�rl�se, ha a k�perny�n k�v�lre ker�l
        if (transform.position.y > deadZone)
        {
            Destroy(gameObject);
        }
    }
}
