using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class intro : MonoBehaviour
{
    public GameObject text;
    public float speed = 5;
    public float deadzone = 1080;
    public TextMeshProUGUI introtext;
    public string textFileName = "IntroText";  
    private string[] lines;
    public bool playeddialog = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset textFile = Resources.Load<TextAsset>("Texts/" + textFileName);
        if (textFile != null)
        {
       
            lines = textFile.text.Split('\n');
        }
        else
        {
            Debug.LogError("Nem található a szövegfájl: " + textFileName);
        }
        ShowTextByKeyword("#INTRO", introtext);

    }
    void Update()
    {
        //transform.position = transform.position + (Vector3.up * speed) * Time.deltaTime;
        //transform.position.y > deadzone &&
        if (playeddialog == false)
        {
            TriggerDialog();
            playeddialog = true;
            //SceneManager.LoadSceneAsync("Level1");

        }
    }
    public void TriggerDialog()
    {
        FindFirstObjectByType<DialogManager>().StartDialog("#TALK1");
    }
    public string GetTextByKeyword(string keyword)
    {
        bool keywordFound = false;
        string resultText = "";

        foreach (string line in lines)
        {
        
            if (line.Trim() == keyword)
            {
                keywordFound = true;
                continue; 
            }

            if (keywordFound)
            {
                if (line.StartsWith("#"))  
                {
                    break;  
                }

                resultText += line + "\n"; 
            }
        }

        return resultText;
    }
    public void ShowTextByKeyword(string keyword, TextMeshProUGUI textmeshname)
    {
        string textToShow = GetTextByKeyword(keyword);
        if (!string.IsNullOrEmpty(textToShow))
        {
            textmeshname.text = textToShow;
        }
        else
        {
            Debug.LogError("Nem található a megadott kulcsszóhoz tartozó szöveg: " + keyword);
        }
    }


}
