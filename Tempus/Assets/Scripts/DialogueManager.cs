using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    
    public Animator DialogueBoxAnim;
    public GameObject DialogueBox;

    private Queue<string> sentences;
    public Dialogue introDialogue;
    void Start()
    {
        sentences = new Queue<string>();
        StartDialogue(introDialogue);
    }

    public void StartDialogue(Dialogue dialogue) 
    {
        DialogueBox.SetActive(true);
        DialogueBoxAnim.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();
        foreach(string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence() 
    {
        if(sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentenceOut(sentence));
    }

    IEnumerator TypeSentenceOut(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    public void EndDialogue() 
    {
        DialogueBoxAnim.SetBool("IsOpen", false);
    }
}
