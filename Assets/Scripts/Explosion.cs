using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifespan = 2f; 

     void Start()
    {
        Destroy(gameObject, lifespan);
    }
}
