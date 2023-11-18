using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D _playerRigidbody;
    Vector2 movement;

    
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMove(InputValue value)
    {
        _playerRigidbody.velocity = value.Get<Vector2>() * moveSpeed;
    }
}
