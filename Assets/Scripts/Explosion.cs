using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifespan = 2f; // A robban�s �lettartama m�sodpercben

    private void Start()
    {
        // T�rl�s a megadott id� ut�n
        Destroy(gameObject, lifespan);
    }
}
