using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float multiplicador = 2f;
    public float duracion = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AplicarSpeedBoost(multiplicador, duracion);
                Destroy(gameObject);
            }
        }
    }
}