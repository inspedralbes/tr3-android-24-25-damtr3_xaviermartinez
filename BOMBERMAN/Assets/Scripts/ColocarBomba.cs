using UnityEngine;
using System.Collections;

public class ColocarBomba : MonoBehaviour
{
    public GameObject bombaPrefab;       // Prefab de la bomba
    public GameObject explosionPrefab;   // Prefab de la explosión
    public float tiempoDeVida = 3f;      // Tiempo antes de explotar
    public float tamañoCelda = 1f;       // Tamaño de la cuadrícula
    private bool bombaActiva = false;    // Control para permitir solo una bomba a la vez
    public LayerMask capaBomba;          // Capa de la bomba (para manejar colisión)

    private Vector2 direccionActual = Vector2.down;  // Dirección por defecto al inicio

    void Update()
    {
        // Detectar la dirección en la que está mirando el personaje basándose en las teclas de dirección
        if (Input.GetKey(KeyCode.W)) direccionActual = Vector2.up;
        if (Input.GetKey(KeyCode.S)) direccionActual = Vector2.down;
        if (Input.GetKey(KeyCode.A)) direccionActual = Vector2.left;
        if (Input.GetKey(KeyCode.D)) direccionActual = Vector2.right;

        if (Input.GetKeyDown(KeyCode.Space) && !bombaActiva)
        {
            Colocar();
        }
    }

    void Colocar()
    {
        Vector3 posicionJugador = transform.position;
        Vector2 posicionAlineada = AlinearAPosicion(posicionJugador);

        // Calcular la nueva posición delante del personaje
        Vector2 posicionBomba = posicionAlineada + direccionActual * tamañoCelda;

        // Instancia la bomba en la nueva posición
        GameObject bomba = Instantiate(bombaPrefab, posicionBomba, Quaternion.identity);
        bombaActiva = true;  // Marca que hay una bomba activa

        // Desactiva la colisión temporalmente entre el jugador y la bomba
        Physics2D.IgnoreCollision(bomba.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

        // Vuelve a habilitar la colisión después de un pequeño tiempo
        StartCoroutine(ActivarColisionDespues(bomba));

        // Inicia la explosión después de tiempoDeVida
        StartCoroutine(Explosión(bomba, posicionBomba));
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        float y = Mathf.Floor(posicion.y / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        return new Vector2(x, y);
    }

    IEnumerator ActivarColisionDespues(GameObject bomba)
    {
        yield return new WaitForSeconds(0.5f);  // Espera para que el jugador se aleje
        Physics2D.IgnoreCollision(bomba.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }

    IEnumerator Explosión(GameObject bomba, Vector2 posicion)
    {
        yield return new WaitForSeconds(tiempoDeVida);

        // Destruye la bomba y habilita colocar una nueva
        Destroy(bomba);
        bombaActiva = false;

        // Instancia la explosión en el centro
        Instantiate(explosionPrefab, posicion, Quaternion.identity);

        // Explosión en las 4 direcciones (arriba, abajo, izquierda, derecha)
        Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 direccion in direcciones)
        {
            for (int i = 1; i <= 3; i++) // Radio de 3 bloques
            {
                Vector2 nuevaPos = posicion + direccion * i * tamañoCelda;

                // Instancia el efecto de explosión
                GameObject explosion = Instantiate(explosionPrefab, nuevaPos, Quaternion.identity);

                // Verifica si hay un objeto destruible
                Collider2D colision = Physics2D.OverlapCircle(nuevaPos, 0.2f);
                if (colision != null && colision.CompareTag("Destruible"))
                {
                    Destroy(colision.gameObject); // Destruye el objeto
                    break; // Detiene la explosión en esa dirección
                }

                yield return new WaitForSeconds(0.1f); // Pequeño delay entre cada explosión
            }
        }
    }
}
