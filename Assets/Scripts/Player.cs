using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5f; // Player mozgási sebessége

    // Update hívódik minden frame-ben
    void Update()
    {
        // Input detektálása a vízszintes (Horizontal) és függõleges (Vertical) tengelyeken
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Mozgás vektor kiszámítása
        Vector3 move = new Vector3(moveX, moveY, 0);

        // Mozgás alkalmazása
        transform.position += move * moveSpeed * Time.deltaTime;
    }

}
