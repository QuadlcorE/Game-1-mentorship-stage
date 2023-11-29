using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] private InputActionReference moveActionToBeUsed;
    [SerializeField] private float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection =  moveActionToBeUsed.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
