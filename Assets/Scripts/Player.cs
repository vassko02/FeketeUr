using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5f; // Player mozgási sebessége

    private Vector2 screenBounds; // A képernyõ szélei
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        // A képernyõ széleinek kiszámítása
        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        // A hajó méretének kiszámítása
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0);
        transform.position += move * moveSpeed * Time.deltaTime;

        // A játékos képernyõn belül tartása
        keepPlayerInBounds();
    }

    void keepPlayerInBounds()
    {
        // Játékos aktuális pozíciója
        Vector3 pos = transform.position;

        // Pozíció korlátozása a képernyõn belülre
        pos.x = Mathf.Clamp(pos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        pos.y = Mathf.Clamp(pos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);

        // A frissített pozíció beállítása
        transform.position = pos;
    }

}
