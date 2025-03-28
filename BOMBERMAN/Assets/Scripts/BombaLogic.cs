using UnityEngine;
using System.Collections;

public class BombaLogic : MonoBehaviour
{
    public float tiempoExplosion = 3f;
    public int radioExplosion = 3;
    public GameObject explosionPrefab;
    public LayerMask capaObstaculos;
    public LayerMask capaDestruible;

    private PlayerMovement jugador;

    public void SetPlayer(PlayerMovement player)
    {
        jugador = player;
        StartCoroutine(Explotar());
    }

    IEnumerator Explotar()
    {
        yield return new WaitForSeconds(tiempoExplosion);

        // Iniciar explosión
        Explosion explosionScript = GetComponent<Explosion>();
        if (explosionScript != null)
        {
            explosionScript.IniciarExplosion(transform.position, radioExplosion, capaObstaculos, capaDestruible);
        }

        // Notificar al jugador y destruir bomba
        jugador.EliminarBomba();
        Destroy(gameObject);
    }
}