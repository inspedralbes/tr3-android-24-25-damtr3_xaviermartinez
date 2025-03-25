using UnityEngine;
using System.Collections;

public class BombaBoost : MonoBehaviour
{
    public float duracion = 5f;  // Duración del boost en segundos
    public int aumentoBombas = 1;  // Cuántas bombas se agregan con el power-up

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("oumaigod");
        // Verifica si el objeto que toca el power-up es el jugador
        if (other.CompareTag("Player"))
        {
            ActivarExtraBombs(other.gameObject);
            Destroy(gameObject);  // Destruye el power-up después de recogerlo
        }
    }

    private void ActivarExtraBombs(GameObject jugador)
    {
        // Obtener el script de PlayerMovement y aumentar las bombas
        PlayerMovement playerMovement = jugador.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.AumentarMaxBombas(aumentoBombas);  // Aumentamos el límite de bombas
            StartCoroutine(DesactivarExtraBombs(playerMovement));  // Desactivamos el boost después de un tiempo
        }
    }

    private IEnumerator DesactivarExtraBombs(PlayerMovement playerMovement)
    {
        // Espera la duración del power-up
        yield return new WaitForSeconds(duracion);

        // Reducimos el límite de bombas al valor original
        playerMovement.ReducirMaxBombas(aumentoBombas);
    }
}
