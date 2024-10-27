using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifespan = 2f; 

    private void Start()
    {
        Destroy(gameObject, lifespan);
    }
}
