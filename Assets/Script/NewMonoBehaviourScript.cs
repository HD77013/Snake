using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeScript : MonoBehaviour
{
    public InputActionReference movement;

    public Vector2 direction = Vector2.right;

    public List<Transform> segments;

    public Transform segmentPrefab;

    public int initialSize = 4;

    public GameManager manager;

    private int score;

    [SerializeField] private bool W = true;
    [SerializeField] private bool A = true;
    [SerializeField] private bool S = true;
    [SerializeField] private bool D = false;

    private Coroutine resetRoutine;

    public bool canContinue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        segments = new List<Transform>();
        segments.Add(this.transform);

        StartingSize();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (movement.action.ReadValue<Vector2>() != Vector2.zero)   // Will keep the player moving even when keys are not pressed
        {
            if (movement.action.ReadValue<Vector2>() == Vector2.up && W)    // Player can't move backwards. This set of code prevents the player from moving opposite to the direction they're travelling
            {
                W = true;
                S = false;
                A = true;
                D = true;
                direction = movement.action.ReadValue<Vector2>();
            }
            if (movement.action.ReadValue<Vector2>() == Vector2.down && S)  // Player can't move up
            {
                W = false;
                S = true;
                A = true;
                D = true;
                direction = movement.action.ReadValue<Vector2>();
            }
            if (movement.action.ReadValue<Vector2>() == Vector2.left && D) // Player can't move to the right
            {
                W = true;
                S = true;
                A = false;
                D = true;
                direction = movement.action.ReadValue<Vector2>();
            }
            if (movement.action.ReadValue<Vector2>() == Vector2.right && A) // Player can't move to the left
            {
                W = true;
                S = true;
                A = true;
                D = false;
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

    private IEnumerator DestroySegments()
    {
        Debug.Log("Coroutine activating");

        direction = Vector2.zero;

        while (segments.Count > 1) // Destroys all segments
        {
            Transform last = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1);  // Removes each segment from list

            if (last != null)
                Destroy(last.gameObject);   // Guard checking for any segments that are already destroyed

            yield return new WaitForSeconds(1f);
        }

        canContinue = true;

        W = false;
        S = true;
        A = false;
        D = false;
    }

    public void RestartSnake()
    {
        if (resetRoutine != null)
        {
            StopCoroutine(resetRoutine);
            resetRoutine = null;
        }

        this.transform.position = Vector3.zero;

        StartingSize();
        GetComponent<Collider2D>().enabled = true;
    }

    public void ResetGame()
    {
        Debug.Log("Reset");

        if (resetRoutine != null)
            StopCoroutine(resetRoutine);

        GetComponent<Collider2D>().enabled = false;

        resetRoutine = StartCoroutine(DestroySegments());

        canContinue = false;

        W = false;
        S = false;
        A = false;
        D = false;
    }

    public void Grow()
    {
        Debug.Log("New seg");

        Transform newSegment = Instantiate(this.segmentPrefab);      // Create a new segment
        newSegment.position = segments[segments.Count - 1].position; // Put at the same place as the current final segment

        segments.Add(newSegment);                                   // Add to list of segments
    }

    public void StartingSize()
    {
        for (int i = 1; i < initialSize; i++)
        {
            segments.Add(Instantiate(this.segmentPrefab));      // Creates segments and add them to the list of segments

            Vector2 sizeDir = Vector2.up;

            segments[i].position = segments[i - 1].position - new Vector3(sizeDir.x, sizeDir.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag) {
            case "Food":
                score++;
                manager.UpdateScore(score);
                Grow();
                break;
            case "Danger":
                Debug.Log("Collision");
                manager.UpdateHighScore();
                score = 0;
                manager.UpdateScore(score);
                ResetGame();
                manager.ResetGame();
                break;
        }

    }
}
