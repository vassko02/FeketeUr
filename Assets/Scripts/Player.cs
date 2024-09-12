using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5f; // Player mozg�si sebess�ge

    private Vector2 screenBounds; // A k�perny� sz�lei
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        // A k�perny� sz�leinek kisz�m�t�sa
        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        // A haj� m�ret�nek kisz�m�t�sa
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0);
        transform.position += move * moveSpeed * Time.deltaTime;

        // A j�t�kos k�perny�n bel�l tart�sa
        keepPlayerInBounds();
    }

    void keepPlayerInBounds()
    {
        // J�t�kos aktu�lis poz�ci�ja
        Vector3 pos = transform.position;

        // Poz�ci� korl�toz�sa a k�perny�n bel�lre
        pos.x = Mathf.Clamp(pos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        pos.y = Mathf.Clamp(pos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);

        // A friss�tett poz�ci� be�ll�t�sa
        transform.position = pos;
    }

}
