using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float duracion = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivarVelocidad(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void ActivarVelocidad(GameObject jugador)
    {
        PlayerMovement playerMovement = jugador.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.velocidad *= 2;
            StartCoroutine(DesactivarVelocidad(playerMovement));
        }
    }

    private IEnumerator DesactivarVelocidad(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(duracion);
        if (playerMovement != null)
            playerMovement.velocidad /= 2;
    }
}
