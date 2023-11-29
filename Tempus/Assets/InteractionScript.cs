using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
}

public class InteractionScript : MonoBehaviour
{
    public Transform InteractorSource;
    private bool interact = false;
    [SerializeField]
    private GameObject currentInteractible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<bool>());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ( collision.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            currentInteractible = collision.gameObject;
            interactObj.Interact();
        }
        else
        {
            currentInteractible = null;
        }
    }

}
