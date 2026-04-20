using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public float fps;

    [SerializeField]TextMeshProUGUI scoreText;
    [SerializeField]TextMeshProUGUI highScoreText;

    public int highScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.fixedDeltaTime = 1f / fps;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score) 
    { 
        scoreText.text = "Score: " + score.ToString();

        PlayerPrefs.SetInt("Score", score);
    }

    public void UpdateHighScore()
    {
        int score = PlayerPrefs.GetInt("Score");

        if (highScore == 0)
        {
            highScore = score;
            highScoreText.text = "High Score: " + score.ToString();
        }

        else if (score > highScore)
        {
            highScoreText.text = "High Score: " + score.ToString();
        }
    }

}
