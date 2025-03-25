using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float duracion = 5f;  // Duración del boost de velocidad en segundos

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que toca el power-up es el jugador
        if (other.CompareTag("Player"))
        {
            ActivarVelocidad(other.gameObject);
            Destroy(gameObject);  // Destruye el power-up después de recogerlo
        }
    }

    private void ActivarVelocidad(GameObject jugador)
    {
        // Aquí debes aumentar la velocidad del jugador
        PlayerMovement playerMovement = jugador.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.velocidad *= 2;  // Duplica la velocidad del jugador
            StartCoroutine(DesactivarVelocidad(playerMovement));  // Restablece la velocidad después de la duración
        }
    }

    private IEnumerator DesactivarVelocidad(PlayerMovement playerMovement)
    {
        // Espera la duración del power-up
        yield return new WaitForSeconds(duracion);

        // Restablece la velocidad del jugador
        if (playerMovement != null)
        {
            playerMovement.velocidad /= 2;  // Restaura la velocidad a su valor original
        }
    }
}
