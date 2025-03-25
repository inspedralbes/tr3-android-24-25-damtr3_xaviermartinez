using UnityEngine;
using System.Collections;

public class BombaBoost : MonoBehaviour
{
    public float duracion = 5f;  // Duraci�n del boost en segundos
    public int aumentoBombas = 1;  // Cu�ntas bombas se agregan con el power-up

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("oumaigod");
        // Verifica si el objeto que toca el power-up es el jugador
        if (other.CompareTag("Player"))
        {
            ActivarExtraBombs(other.gameObject);
            Destroy(gameObject);  // Destruye el power-up despu�s de recogerlo
        }
    }

    private void ActivarExtraBombs(GameObject jugador)
    {
        // Obtener el script de PlayerMovement y aumentar las bombas
        PlayerMovement playerMovement = jugador.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.AumentarMaxBombas(aumentoBombas);  // Aumentamos el l�mite de bombas
            StartCoroutine(DesactivarExtraBombs(playerMovement));  // Desactivamos el boost despu�s de un tiempo
        }
    }

    private IEnumerator DesactivarExtraBombs(PlayerMovement playerMovement)
    {
        // Espera la duraci�n del power-up
        yield return new WaitForSeconds(duracion);

        // Reducimos el l�mite de bombas al valor original
        playerMovement.ReducirMaxBombas(aumentoBombas);
    }
}
