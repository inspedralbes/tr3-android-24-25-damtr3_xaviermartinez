using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar escenas

public class SceneLoader : MonoBehaviour
{
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("Register");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game"); // Reinicia el juego desde la escena "Game"
    }
}
