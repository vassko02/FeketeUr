using UnityEngine;

public class BlackOverlay : MonoBehaviour
{
    public Transform player; // A player transformja, amit k�vetni fogunk

    void Start()
    {
        if (player == null)
        {
            // Megpr�b�ljuk automatikusan megtal�lni a j�t�kost
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // A fekete overlay k�veti a j�t�kos poz�ci�j�t az eltol�ssal egy�tt
            transform.position = player.position;
        }
    }
}
