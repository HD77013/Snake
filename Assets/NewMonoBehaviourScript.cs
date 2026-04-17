using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeScript : MonoBehaviour
{
    public InputActionReference movement;

    public Vector2 direction = Vector2.right;

    public List<Transform> segments;

    public Transform segmentPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        segments = new List<Transform>();
        segments.Add(this.transform);
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
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;    // Moves segments
        }

        transform.position = new Vector3(       // Rounds values to ensure player moves along grid
            Mathf.Round(transform.position.x + direction.x),
            Mathf.Round(transform.position.y + direction.y),
            0.0f
        );        
    }

    public void Grow()
    {
        Transform newSegment = Instantiate(this.segmentPrefab);      // Create a new segment
        newSegment.position = segments[segments.Count - 1].position; // Put at the same place as the current final segment

        segments.Add(newSegment);                                   // Add to list of segments
    }

    public void ResetGame()
    {
       for (int i = segments.Count - 1; i > 0; i--) // Destroys all segments
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();             // Clears segments in list
        segments.Add(this.transform); // Adds the snakes head back

        this.transform.position = Vector3.zero;
        direction = Vector2.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag) {
            case "Food":
                Grow();
                break;
            case "Danger":
                ResetGame();
                break;
        }

    }
}
