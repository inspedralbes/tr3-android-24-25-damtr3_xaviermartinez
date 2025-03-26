using UnityEngine;
using System.Collections;

public class ColocarBomba : MonoBehaviour
{
    public GameObject bombaPrefab;
    public float tiempoDeVida = 3f;
    public float tamañoCelda = 1f;
    private bool bombaActiva = false;
    public LayerMask capaBomba;         // Capa para detectar bombas
    public LayerMask capaObstaculos;    // Capa para detectar paredes u obstáculos

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !bombaActiva && GetComponent<PlayerMovement>().bombasColocadas < GetComponent<PlayerMovement>().maxBombas)
        {
            Colocar();
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
        GameObject bomba = Instantiate(bombaPrefab, posicion, Quaternion.identity);
        bombaActiva = true;

        // Ignorar colisión temporalmente
        Physics2D.IgnoreCollision(bomba.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        StartCoroutine(ActivarColisionDespues(bomba));

        StartCoroutine(Explosión(bomba, posicion));  // Iniciar cuenta atrás de explosión
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
        Destroy(bomba);
        bombaActiva = false;

        Explosion explosionScript = bomba.GetComponent<Explosion>();
        if (explosionScript != null)
        {
            explosionScript.IniciarExplosión(posicion);
        }
    }
}
