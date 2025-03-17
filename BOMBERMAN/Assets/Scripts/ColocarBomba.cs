using UnityEngine;

public class ColocarBomba : MonoBehaviour
{
    public GameObject bombaPrefab;  // Prefab de la bomba
    public float tiempoDeVida = 3f; // Tiempo antes de explotar
    public float tamañoCelda = 1f;  // Tamaño de cada celda del grid

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Colocar();
        }
    }

    void Colocar()
    {
        // Redondea la posición del jugador al centro de la celda más cercana
        Vector3 posicionJugador = transform.position;
        float xAjustado = Mathf.Round(posicionJugador.x / tamañoCelda) * tamañoCelda;
        float yAjustado = Mathf.Round(posicionJugador.y / tamañoCelda) * tamañoCelda;

        // Instancia la bomba en la posición ajustada
        GameObject bomba = Instantiate(bombaPrefab, new Vector3(xAjustado, yAjustado, 0), Quaternion.identity);

        // Destruye la bomba después de un tiempo
        Destroy(bomba, tiempoDeVida);
    }
}
