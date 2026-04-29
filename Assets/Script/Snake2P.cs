using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake2P : MonoBehaviour
{
    public InputActionReference movement;

    public Vector2 direction;

    public List<Transform> segments;

    public Transform segmentPrefab;

    public int initialSize = 4;

    public TwoPManager manager;

    private int score;

    [SerializeField] private bool W = true;
    [SerializeField] private bool A = true;
    [SerializeField] private bool S = false;
    [SerializeField] private bool D = true;

    private Coroutine resetRoutine;

    private bool isResetting;

    public bool canContinue;

    public Vector2 allocatedPos;

    public GameObject otherPLayerOBJ;
    public Snake2P otherPlayer;

    public bool playerDied;

    public SpriteRenderer head;

    [Header("Particles")]
    [SerializeField] private ParticleSystem deathParticle;
    private ParticleSystem particleInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        segments = new List<Transform>();
        segments.Add(this.transform);

        direction = Vector2.zero;

        manager.StartFade();
        manager.waiting = true;
        canContinue = true;

        StartingSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (isResetting) return;

        if (movement.action.ReadValue<Vector2>() != Vector2.zero)   // Will keep the player moving even when keys are not pressed
        {
            if (movement.action.ReadValue<Vector2>() == Vector2.up && W)    // Player can't move backwards. This set of code prevents the player from moving opposite to the direction they're travelling
            {
                W = true; S = false; A = true; D = true;
                direction = movement.action.ReadValue<Vector2>();
            }
            if (movement.action.ReadValue<Vector2>() == Vector2.down && S)  // Player can't move up
            {
                W = false; S = true; A = true; D = true;
                direction = movement.action.ReadValue<Vector2>();
            }
            if (movement.action.ReadValue<Vector2>() == Vector2.left && D) // Player can't move to the right
            {
                W = true;S = true; A = false; D = true;
                direction = movement.action.ReadValue<Vector2>();
            }
            if (movement.action.ReadValue<Vector2>() == Vector2.right && A) // Player can't move to the left
            {
                W = true; S = true; A = true; D = false;
                direction = movement.action.ReadValue<Vector2>();
            }

        }

    }

    void FixedUpdate()
    {
        if (isResetting) return;
        if (direction == Vector2.zero) return;  // Doesn't update segments when stationary

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

    public void Freeze()
    {
        direction = Vector2.zero;
        isResetting = true;
        W = false; S = false; A = false; D = false;
    }

    public void NewGame()
    {
        // Clear any leftover body segments (keeps the head)
        for (int i = segments.Count - 1; i >= 1; i--)
        {
            Destroy(segments[i].gameObject);
            segments.RemoveAt(i);
        }

        segments[0].GetComponent<SpriteRenderer>().enabled = true;

        W = true; S = true; A = true; D = true;

        direction = Vector2.zero;
        playerDied = false;
        transform.position = allocatedPos;

        StartingSize();

        GetComponent<Collider2D>().enabled = true;
        isResetting = false;
        canContinue = true;

        head.enabled = true;
    }


    private IEnumerator DestroySegments()
    {
        direction = Vector2.zero;

        yield return new WaitForSeconds(0.5f);  // Freezes the player. Give them time to realize "oh shit. I hit the wall or other player"

        for (int i = segments.Count - 1; i > 0; i--)    // Death FX
        {
            particleInstance = Instantiate(deathParticle, segments[i].position, Quaternion.identity);

            Transform last = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1);

            if (last != null)
                Destroy(last.gameObject);

            yield return new WaitForSeconds(0.05f);
        }

        Instantiate(deathParticle, segments[0].position, Quaternion.identity);
        segments[0].GetComponent<SpriteRenderer>().enabled = false;


        // Tell the manager the death FX is done — it will handle resetting both snakes
        manager.OnDeathSequenceComplete();

    }

    public void RestartSnake()
    {
        if (resetRoutine != null)
        {
            StopCoroutine(resetRoutine);
            resetRoutine = null;
        }
    }

    public void ResetGame()
    {
        isResetting = true;
        canContinue = false;

        if (resetRoutine != null)
            StopCoroutine(resetRoutine);

        GetComponent<Collider2D>().enabled = false;

        W = false; S = false; A = false; D = false;

        resetRoutine = StartCoroutine(DestroySegments());
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
        switch (collision.tag)
        {
            case "Food":
                score++;
                manager.UpdateScore(score);
                Grow();
                break;
            case "Danger":
                manager.UpdateHighScore();
                score = 0;
                manager.UpdateScore(score);
                playerDied = true;
                manager.PlayerDied(this);   // Hand off to Manager to coordinate both snakes
                break;
        }

    }
}
