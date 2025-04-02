using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class AuthManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:3001/api/auth"; // URL de tu backend

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedbackText; // Para mostrar mensajes de error o éxito

    public void Register()
    {
        StartCoroutine(RegisterCoroutine(usernameInput.text, passwordInput.text));
    }

    public void Login()
    {
        StartCoroutine(LoginCoroutine(usernameInput.text, passwordInput.text));
    }

    IEnumerator RegisterCoroutine(string username, string password)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/register", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                feedbackText.text = "Registro exitoso. ¡Ahora inicia sesión!";
            }
            else
            {
                feedbackText.text = "Error en el registro: " + request.downloadHandler.text;
            }
        }
    }

    IEnumerator LoginCoroutine(string username, string password)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string token = request.downloadHandler.text;
                PlayerPrefs.SetString("authToken", token); // Guarda el token para futuras solicitudes
                feedbackText.text = "Inicio de sesión exitoso";
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game"); // Cargar la escena del juego
            }
            else
            {
                feedbackText.text = "Error en el login: " + request.downloadHandler.text;
            }
        }
    }
}
