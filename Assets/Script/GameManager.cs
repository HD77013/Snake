using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.VersionControl.Asset;
using static UnityEngine.Rendering.HableCurve;

public class GameManager : MonoBehaviour
{
    public SnakeScript snake;
    public UIFade fade;

    public float fps;

    [SerializeField]TextMeshProUGUI scoreText;
    [SerializeField]TextMeshProUGUI highScoreText;

    public int highScore;

    public bool waiting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.fixedDeltaTime = 1f / fps;
    }

    // Update is called once per frame
    void Update()
    {
        if (snake.movement.action.WasPressedThisFrame() && waiting && snake.canContinue)
        {
            Debug.Log("Reseting Game");
            snake.canContinue = false;
            waiting = false;
            fade.StopRoutine();
            snake.RestartSnake();
        }

    }

    public void StartFade()
    {
        fade.StartRoutine();
    }

    public void ResetGame()
    {
        if (!waiting) 
        {
            waiting = true;
        }
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
