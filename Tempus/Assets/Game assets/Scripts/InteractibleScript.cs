using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleScript : MonoBehaviour, IInteractable
{
    public SpriteRenderer spriteRenderer;
    public GameObject dialogueManager;
    public Color tint; // The tint color
    public float speed = 1f; // The speed of the oscillation
    public DialogueManager dialogueManagerScript;

    void Start()
    {
        // dialogueManagerScript = dialogueManager.GetComponent<DialogueManager>();
    }

    void Update()
    {
        // TODO Change the color of the sprite
    }

    public void Interact()
    {
        dialogueManagerScript.StartDialogue(dialogueManagerScript.introDialogue);
        Debug.Log("Interacted with blue box");
    }
}
