using UnityEngine;
using System.Collections;

public class BombaBoost : MonoBehaviour
{
    public float duracion = 5f;
    public int aumentoBombas = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Power-up de bombas recogido.");
        if (other.CompareTag("Player"))
        {
            ActivarExtraBombs(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void ActivarExtraBombs(GameObject jugador)
    {
        PlayerMovement playerMovement = jugador.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.AumentarMaxBombas(aumentoBombas);
            StartCoroutine(DesactivarExtraBombs(playerMovement));
        }
    }

    private IEnumerator DesactivarExtraBombs(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(duracion);
        playerMovement.ReducirMaxBombas(aumentoBombas);
    }
}
