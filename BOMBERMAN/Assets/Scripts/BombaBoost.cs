using UnityEngine;

public class BombaBoost : MonoBehaviour
{
    public int bombasExtra = 1;
    public float duracion = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AplicarBombaBoost(bombasExtra, duracion);
                Destroy(gameObject);
            }
        }
    }
}