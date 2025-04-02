using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class AuthManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:3001/api/auth"; // URL de tu backend

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nameInput;  // Nuevo campo para el nombre
    public TextMeshProUGUI feedbackText; // Para mostrar mensajes de error o éxito

    // Método para el registro
    public void Register()
    {
        StartCoroutine(RegisterCoroutine(usernameInput.text, passwordInput.text, nameInput.text)); // Enviar también el nombre
    }

    // Método para el login
    public void Login()
    {
        StartCoroutine(LoginCoroutine(usernameInput.text, passwordInput.text));
    }

    // Coroutine para el registro
    IEnumerator RegisterCoroutine(string username, string password, string name)
    {
        Debug.Log("RegisterCoroutine");
        Debug.Log(usernameInput.text);
        Debug.Log(passwordInput.text);
        Debug.Log(nameInput.text);

        // Se agrega el nombre al JSON
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\",\"name\":\"{name}\"}}";
        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/register", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            Debug.Log("VOY A MANDARME");
            yield return request.SendWebRequest();
            Debug.Log("HE PASADO");
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

    // Coroutine para el login
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
