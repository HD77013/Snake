using UnityEngine;
using UnityEngine.Rendering;

public class Food : MonoBehaviour
{

    public BoxCollider2D grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomizePosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomizePosition()
    {
        Bounds bounds = grid.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                RandomizePosition();
                break;
        }
    }
}
