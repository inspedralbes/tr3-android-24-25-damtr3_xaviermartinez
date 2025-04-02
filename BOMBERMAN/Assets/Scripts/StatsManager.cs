using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class StatsManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:3001/api/stats";
    void Start()
    {
        // Inicia la corrutina para enviar datos automáticamente después de X segundos
        StartCoroutine(AutoSendStats());
    }

    IEnumerator AutoSendStats()
    {
        yield return new WaitForSeconds(10); // Espera 10 segundos antes de enviar las estadísticas
        SendStats("Jugador1", 10, 5, 1);
    }

    public void SendStats(string player, int blocksDestroyed, int bombsPlaced, int gamesPlayed)
    {
        StartCoroutine(SendStatsCoroutine(player, blocksDestroyed, bombsPlaced, gamesPlayed));
    }

    private IEnumerator SendStatsCoroutine(string player, int blocksDestroyed, int bombsPlaced, int gamesPlayed)
    {
        string jsonData = $"{{\"player\":\"{player}\",\"stats\":{{\"blocksDestroyed\":{blocksDestroyed},\"bombsPlaced\":{bombsPlaced},\"gamesPlayed\":{gamesPlayed}}}}}";

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Estadísticas enviadas automáticamente: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al enviar estadísticas: " + request.error);
            }
        }
    }
}
