using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class intro : MonoBehaviour
{
    public GameObject text;
    public float speed = 5;
    public float deadzone = 1080;
    public Image maincharacter;
    public GameObject textbubble;
    public TextMeshProUGUI charactertext;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.up * speed) * Time.deltaTime;
        if (transform.position.y > deadzone)
        {
            maincharacter.gameObject.SetActive(true);
            textbubble.gameObject.SetActive(true);
            charactertext.text = "A kurva anyad kutya kalman";
            //SceneManager.LoadSceneAsync("Level1");

        }
    }
}
