using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D _playerRigidbody;
    Vector2 movement;
    Vector2 direction;
    float targetAngle;

    
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // This is the best I could come up with for now Doesn't work at all sha
        /*
        direction = _playerRigidbody.velocity.normalized;
        targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle - 90));
        */
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _playerRigidbody.velocity = ctx.ReadValue<Vector2>() * moveSpeed;
    }
}
