using UnityEditor.Build;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Lövedék sebessége
    public float deadZone = 7; // Az a zóna, ahol a lövedéket töröljük
    public Vector3 direction = Vector3.right; // Az irány, amerre a lövedék mozog
    public GameObject explosion;
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
        Debug.Log(collision.gameObject.tag);
        if ((gameObject.tag == "PlayerProjectile" && collision.gameObject.tag == "Player")||(collision.gameObject.tag=="Asteroid")||collision.gameObject.tag=="EnemyProjectile" || collision.gameObject.tag == "PlayerProjectile") { }
        else
        {
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosionInstance, 3f); // Például ha 3 másodpercig tart az animáció

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
            if (collision.gameObject.CompareTag("Kronis"))
            {
                Kronis kronis = collision.gameObject.GetComponent<Kronis>();
                if (kronis != null)
                {
                    kronis.TakeDamage(bulletDamage);
                }
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Orion"))
            {
                Orion orion = collision.gameObject.GetComponent<Orion>();
                if (orion != null)
                {
                    orion.TakeDamage(bulletDamage);
                }
                Destroy(gameObject);
            }
        }      
    }
}