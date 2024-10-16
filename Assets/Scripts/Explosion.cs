using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifespan = 2f; // A robbanás élettartama másodpercben

    private void Start()
    {
        // Törlés a megadott idõ után
        Destroy(gameObject, lifespan);
    }
}
