using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleScript : MonoBehaviour, IInteractable
{
    public SpriteRenderer spriteRenderer;
    public Color tint; // The tint color
    public float speed = 1f; // The speed of the oscillation

    void Update()
    {
        // TODO Change the color of the sprite
    }

    public void Interact()
    {
        Debug.Log("Interacted with blue box");
    }
}
