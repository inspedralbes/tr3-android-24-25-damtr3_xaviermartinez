using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float multiplicador = 2f; // Velocidad x2
    public float duracion = 5f;      // Duración del efecto

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AplicarSpeedBoost(multiplicador, duracion);
                Destroy(gameObject); // Destruye el power-up
            }
        }
    }
}