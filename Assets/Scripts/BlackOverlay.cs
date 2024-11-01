using UnityEngine;

public class BlackOverlay : MonoBehaviour
{
    public Transform player; // A player transformja, amit követni fogunk

    void Start()
    {
        if (player == null)
        {
            // Megpróbáljuk automatikusan megtalálni a játékost
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // A fekete overlay követi a játékos pozícióját az eltolással együtt
            transform.position = player.position;
        }
    }
}
