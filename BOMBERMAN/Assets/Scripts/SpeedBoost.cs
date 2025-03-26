using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float duracion = 5f;  // Duraci�n del boost

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el jugador colision� con el boost (usamos "Player" como tag aqu�)
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha recogido el boost"); // Esto se imprime para confirmar que la colisi�n se detecta
            ActivarVelocidad(other.gameObject);
            Destroy(gameObject);  // Destruye el boost al recogerlo
        }
        else
        {
            Debug.Log("No es el jugador: " + other.tag); // Para depurar si hay otro objeto tocando el boost
        }
    }

    private void ActivarVelocidad(GameObject jugador)
    {
        PlayerMovement playerMovement = jugador.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.AumentarVelocidad();  // Aumenta la velocidad del jugador
            StartCoroutine(DesactivarVelocidad(playerMovement));  // Desactivamos el boost despu�s de la duraci�n
        }
        else
        {
            Debug.Log("No se encontr� el componente PlayerMovement en el jugador.");
        }
    }

    private IEnumerator DesactivarVelocidad(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(duracion);
        if (playerMovement != null)
        {
            playerMovement.RestaurarVelocidad();  // Restauramos la velocidad original
        }
    }
}
