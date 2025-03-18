using UnityEngine;
using System.Collections;

public class ColocarBomba : MonoBehaviour
{
    public GameObject bombaPrefab;     // Prefab de la bomba
    public GameObject explosionPrefab; // Prefab de la explosión
    public float tiempoDeVida = 3f;    // Tiempo antes de explotar
    public float tamañoCelda = 1f;     // Tamaño de la cuadrícula

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Colocar();
        }
    }

    void Colocar()
    {
        Vector3 posicionJugador = transform.position;
        Vector2 posicionAlineada = AlinearAPosicion(posicionJugador);

        // Instancia la bomba en la posición ajustada
        GameObject bomba = Instantiate(bombaPrefab, posicionAlineada, Quaternion.identity);

        // Inicia la explosión después de tiempoDeVida
        StartCoroutine(Explosión(bomba, posicionAlineada));
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        float y = Mathf.Floor(posicion.y / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        return new Vector2(x, y);
    }

    IEnumerator Explosión(GameObject bomba, Vector2 posicion)
    {
        yield return new WaitForSeconds(tiempoDeVida);

        // Destruye la bomba
        Destroy(bomba);

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
