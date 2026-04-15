using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public InputActionReference movement;

    public Vector2 direction = Vector2.right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (movement.action.ReadValue<Vector2>() != Vector2.zero)   // Will keep the player moving even when keys are not pressed
        {
            if (movement.action.ReadValue<Vector2>() == Vector2.up || movement.action.ReadValue<Vector2>() == Vector2.down ||
                movement.action.ReadValue<Vector2>() == Vector2.right || movement.action.ReadValue<Vector2>() == Vector2.left)
            {
                direction = movement.action.ReadValue<Vector2>();
            }

        }

    }

    void FixedUpdate()
    {
        transform.position = new Vector3(       // Rounds values to ensure player moves along grid
            Mathf.Round(transform.position.x + direction.x),
            Mathf.Round(transform.position.y + direction.y),
            0.0f
        );        
    }
}
