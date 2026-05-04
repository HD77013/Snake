using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void Solo() => SceneManager.LoadScene("Solo");
    

    public void TwoPlayer() => SceneManager.LoadScene("Name");
    
}
