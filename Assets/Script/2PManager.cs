using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoPManager : MonoBehaviour
{
    public float fps;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    public UIFade fade;

    public int highScore;

    public bool waiting;

    public GameObject announcer;
    public TextMeshProUGUI announcerText;

    public GameObject plr1;
    public GameObject plr2;

    public Snake2P snake1;
    public Snake2P snake2;

    private Snake2P survivor;

    public Vector2 player1Pos = new Vector2(-10, 0);
    public Vector2 player2Pos = new Vector2(10, 0);

    public string Player1Name;
    public string Player2Name;

    public void InputPlayerNames(string name1, string name2)
    {
        Player1Name = name1;
        Player2Name = name2;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.fixedDeltaTime = 1f / fps;

        string currentSceneName = SceneManager.GetActiveScene().name;


        snake1 = plr1.GetComponent<Snake2P>();
        snake2 = plr2.GetComponent<Snake2P>();

        snake1.allocatedPos = player1Pos;
        snake2.allocatedPos = player2Pos;
    }

    void Update()
    {
        if (snake1.movement.action.WasPressedThisFrame() && waiting && snake1.canContinue)
        {
            Debug.Log("Reseting Game");
            snake1.canContinue = false;
            waiting = false;
            fade.StopRoutine();
            snake1.RestartSnake();
        }

        if (snake2.movement.action.WasPressedThisFrame() && waiting && snake2.canContinue)
        {
            Debug.Log("Reseting Game");
            snake2.canContinue = false;
            waiting = false;
            fade.StopRoutine();
            snake2.RestartSnake();
        }

    }

    // Called when a snake hits a Danger collider
    public void PlayerDied(Snake2P dyingPlayer)
    {
        // Figure out which snake survived
        survivor = (dyingPlayer == snake1) ? snake2 : snake1;

        // Freeze the survivor so they can't move during the death FX
        survivor.Freeze();

        // Kick off the dying snake's segment destruction
        dyingPlayer.ResetGame();

        waiting = false; // Block input until reset is complete
    }

    // Called by the dying snake once its DestroySegments coroutine finishes
    public void OnDeathSequenceComplete()
    {
        ShowWinner(survivor);
    }

    private void ShowWinner(Snake2P winner)
    {
        // Set your UI text based on who survived
        if (winner == snake1)
            announcerText.text = "<color=red>" + Player1Name + "</color>" + " Wins!";
        else
            announcerText.text = "<color=green>" + Player2Name + "</color>" + " Wins!";

        announcer.SetActive(true);

        // Hide it again after a few seconds
        Invoke("HideWinner", 3f);
    }

    private void HideWinner()
    {
        announcer.SetActive(false);

        snake1.NewGame();
        snake2.NewGame();

        // Now show the "press to start" prompt for both players
        StartFade();
        waiting = true;
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
