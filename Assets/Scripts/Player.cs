using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5f; // Player mozg�si sebess�ge

    // Update h�v�dik minden frame-ben
    void Update()
    {
        // Input detekt�l�sa a v�zszintes (Horizontal) �s f�gg�leges (Vertical) tengelyeken
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Mozg�s vektor kisz�m�t�sa
        Vector3 move = new Vector3(moveX, moveY, 0);

        // Mozg�s alkalmaz�sa
        transform.position += move * moveSpeed * Time.deltaTime;
    }

}
