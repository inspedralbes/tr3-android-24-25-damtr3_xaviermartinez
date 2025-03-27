using UnityEngine;
using System.Collections;

public class ColocarBomba : MonoBehaviour
{
    public GameObject bombaPrefab;
    public float tiempoDeVida = 3f;
    public float tamañoCelda = 1f;
    public LayerMask capaBomba;         // Capa para detectar bombas
    public LayerMask capaObstaculos;    // Capa para detectar paredes u obstáculos

    void Update()
    {
        PlayerMovement player = GetComponent<PlayerMovement>();

        if (Input.GetKeyDown(KeyCode.Space) && bombaPrefab != null
            && player.bombasColocadas < player.maxBombas)
        {
            Colocar();
        }
        else if (bombaPrefab == null)
        {
            Debug.LogError("¡El prefab de la bomba no está asignado en el Inspector!");
        }
    }

    void Colocar()
    {
        Vector2 posicionAlineada = AlinearAPosicion(transform.position);

        // Verificar si ya hay una bomba en la posición alineada
        if (!Physics2D.OverlapCircle(posicionAlineada, 0.2f, capaBomba))
        {
            InstanciarBomba(posicionAlineada);
        }
    }

    private void InstanciarBomba(Vector2 posicion)
    {
        if (bombaPrefab != null)
        {
            GameObject bomba = Instantiate(bombaPrefab, posicion, Quaternion.identity);

            // Actualizar el contador de bombas
            PlayerMovement player = GetComponent<PlayerMovement>();
            player.bombasColocadas++;

            // Ignorar colisión temporalmente
            Collider2D bombaCollider = bomba.GetComponent<Collider2D>();
            if (bombaCollider != null)
            {
                Physics2D.IgnoreCollision(bombaCollider, GetComponent<Collider2D>(), true);
            }
            StartCoroutine(ActivarColisionDespues(bomba));

            StartCoroutine(Explosión(bomba, posicion));  // Iniciar cuenta atrás de explosión
        }
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        float y = Mathf.Floor(posicion.y / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        return new Vector2(x, y);
    }

    IEnumerator ActivarColisionDespues(GameObject bomba)
    {
        yield return new WaitForSeconds(0.5f);
        if (bomba != null && bomba.GetComponent<Collider2D>() != null)
        {
            Physics2D.IgnoreCollision(bomba.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
    }

    IEnumerator Explosión(GameObject bomba, Vector2 posicion)
    {
        yield return new WaitForSeconds(tiempoDeVida);

        if (bomba != null) // Verificar si la bomba sigue existiendo antes de destruirla
        {
            Destroy(bomba);

            // Reducir el contador de bombas
            PlayerMovement player = GetComponent<PlayerMovement>();
            player.bombasColocadas--;

            Explosion explosionScript = bomba.GetComponent<Explosion>();
            if (explosionScript != null)
            {
                explosionScript.IniciarExplosión(posicion);
            }
        }
    }
}
