using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float fps;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.fixedDeltaTime = 1f / fps;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
