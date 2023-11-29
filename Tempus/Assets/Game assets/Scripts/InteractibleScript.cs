using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleScript : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacted with blue box");
    }
}
