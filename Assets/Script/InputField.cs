using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputField : MonoBehaviour
{
    public TwoPManager manager;

    public TMP_InputField player1Name;
    public TMP_InputField player2Name;

    public string player1;
    public string player2;

    public GameObject player1Input;
    public GameObject player2Input;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Player1NameInput()
    {
        player1 = player1Name.text;

        player1Input.SetActive(false);
        player2Input.SetActive(true);
    }

    public void Player2NameInput()
    {
        player2 = player2Name.text;

        if (SceneManager.GetActiveScene().name != "1v1")
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("1v1");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        manager = FindAnyObjectByType<TwoPManager>();

        if (manager != null)
        {
            manager.InputPlayerNames(player1, player2);
        }
    }
}
