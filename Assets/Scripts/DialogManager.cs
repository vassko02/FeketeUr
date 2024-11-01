using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;



public class DialogManager : MonoBehaviour
{
    public string textfilename;

    public TextMeshProUGUI dialogText;
    private Queue<string> sentences;
    public string currentTalkKey;
    public GameObject Descision;

    public GameObject textbubble;
    public GameObject rightCharacter;
    public GameObject leftCharacter;
    public Transform parent;

    private Dialog dialog;
    public ProgressManager progressManager;

    private GameObject characterInstance;
    private GameObject characterInstance2;
    private GameObject textbubbleinstance;

    private bool DoneWithDescision = false;
    private void OnEnable()
    {
        dialogText.gameObject.SetActive(true);
        sentences = new Queue<string>();

        LoadDialogFromResources(textfilename);
        StartDialog();
    }
    public void StartDialog()
    {
        if (dialog == null)
        {
            Debug.LogError("Dialog is not set!");
            return;
        }

        characterInstance = Instantiate(rightCharacter, parent);
        characterInstance.transform.localPosition = new Vector3(650, -180, 0);

        characterInstance2 = Instantiate(leftCharacter, parent);
        characterInstance2.transform.localPosition = new Vector3(-600, -180, 0);

        textbubbleinstance = Instantiate(textbubble, parent);
        textbubbleinstance.transform.localPosition = new Vector3(20, 256, 0);
        textbubbleinstance.transform.SetAsFirstSibling();


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
        if (sentences.Count>0)
        {
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Typsentence(sentence));
        }
    }

    IEnumerator Typsentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.05f); // 0.05 másodperces késleltetés minden karakter között
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
                    textbubbleinstance.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {

                    textbubbleinstance.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                DisplayNextSentence();
            }
            else
            {
                if (currentTalkKey!="#AFTERNYX"||DoneWithDescision)
                {
                    ContinueGame();
                }
                else
                {
                    Descision.SetActive(true);
                }
            }
        }
    }
    private void ContinueGame()
    {
        Destroy(characterInstance);
        Destroy(characterInstance2);
        Destroy(textbubbleinstance);
        progressManager.midDialog = false;
        progressManager.doneWithDialog = true;
        dialogText.gameObject.SetActive(false);
        gameObject.SetActive(false);
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

    public void Choose(string choice)
    {
        currentTalkKey = choice;
        DoneWithDescision = true;

        Destroy(characterInstance);
        Destroy(characterInstance2);
        Destroy(textbubbleinstance);

        LoadDialogFromResources(textfilename);
        Descision.SetActive(false);
        StartDialog();
    }
}
