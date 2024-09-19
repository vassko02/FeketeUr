using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public GameObject textbubble;

    public TextMeshProUGUI dialogText;
    private Queue<string> sentences;
    public string currentTalkKey;
    public GameObject rightCharacter;
    public GameObject leftCharacter;
    public Transform parent;
    public Dialog dialog;
    public string nextScene;
    public void Start()
    {
        sentences = new Queue<string>();

        LoadDialogFromResources("IntroText");
    }
    public void StartDialog(string talkKey)
    {
        if (dialog == null)
        {
            Debug.LogError("Dialog is not set!");
            return;
        }

        currentTalkKey = talkKey;
        GameObject characterInstance = Instantiate(rightCharacter, parent);
        characterInstance.transform.localPosition = new Vector3(650, -180, 0);

        GameObject characterInstance2 = Instantiate(leftCharacter, parent);
        characterInstance2.transform.localPosition = new Vector3(-600, -180, 0);

        sentences.Clear();

        if (dialog != null)
        {
            foreach (string sentence in dialog.sentences)
            {
               
                    sentences.Enqueue(sentence);
             
            }

            DisplayNextSentence();
        }
        else
        {
            Debug.LogError("Dialog is null.");
        }
    }
    public void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(Typsentence(sentence));
    }

    IEnumerator Typsentence (string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }
  
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (sentences.Count > 0) 
            {

                if (sentences.Peek().Split(':')[0] != "Ryder")
                {
                    textbubble.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {

                    textbubble.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                DisplayNextSentence();


               
            }
            else
            {
                SceneManager.LoadSceneAsync(nextScene);

            }
        }
        

    }
    void LoadDialogFromResources(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Texts/" + fileName);
        if (textAsset == null)
        {
            Debug.LogError("File not found in Resources/Texts");
            return;
        }

        string[] lines = textAsset.text.Split('\n');

        dialog = new Dialog();
        List<string> sentencesList = new List<string>();
        bool insideTargetSection = false;
        foreach (string line in lines)
        {
     
            if (line.StartsWith(currentTalkKey))
            {
                insideTargetSection = true;
                continue;  
            }

            if (line.StartsWith("#") && insideTargetSection)
            {
                insideTargetSection = false;
                break;  
            }

            
            if (insideTargetSection)
            {
                sentencesList.Add(line);
            }
        }

        dialog.sentences = sentencesList.ToArray();
    }

}
