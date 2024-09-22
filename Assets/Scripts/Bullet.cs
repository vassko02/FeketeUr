using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Lövedék sebessége
    public float deadZone = 7; // Az a zóna, ahol a lövedéket töröljük
    public Vector3 direction = Vector3.right; // Az irány, amerre a lövedék mozog
    private int bulletDamage;
    void Update()
    {

        // A lövedék mozgatása az irány szerint
        transform.Translate(direction * speed * Time.deltaTime);

        // A lövedék törlése, ha a képernyőn kívülre kerül
        if (deadZone > 0)
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
    public void SetDamage(int damage)
    {
        bulletDamage = damage; // Sebzés beállítása
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ha az objektum amivel ütközött az Enemy taggel rendelkezik
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Megkeresi az Enemy scriptet az objektumon
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Ha van ilyen script, meghívja a TakeDamage függvényt
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }

            // Lövedék törlése ütközés után
            Destroy(gameObject);
        }
    }
}