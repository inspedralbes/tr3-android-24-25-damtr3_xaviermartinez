using UnityEngine;

public class ColocarBomba : MonoBehaviour
{
    public GameObject bombaPrefab;  // Prefab de la bomba
    public float tiempoDeVida = 3f; // Tiempo antes de explotar
    public float tamañoCelda = 1f;  // Tamaño de la cuadrícula (DEBE ser igual al del personaje)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Colocar();
        }
    }

    void Colocar()
    {
        // Ajusta la posición a la cuadrícula
        Vector3 posicionJugador = transform.position;
        Vector2 posicionAlineada = AlinearAPosicion(posicionJugador);

        // Instancia la bomba en la posición ajustada
        GameObject bomba = Instantiate(bombaPrefab, posicionAlineada, Quaternion.identity);

        // Destruye la bomba después de un tiempo
        Destroy(bomba, tiempoDeVida);
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        float y = Mathf.Floor(posicion.y / tamañoCelda) * tamañoCelda + tamañoCelda / 2;
        return new Vector2(x, y);
    }
}
