using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public GameObject explosionPrefab;   // Prefab de la explosión
    public float tamañoCelda = 1f;       // Tamaño de la cuadrícula
    public int radioExplosion = 3;       // Radio de bloques afectados por la explosión

    public void IniciarExplosión(Vector2 posicion)
    {
        // Instancia la explosión en el centro
        Instantiate(explosionPrefab, posicion, Quaternion.identity);

        // Explosión en las 4 direcciones (arriba, abajo, izquierda, derecha)
        Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 direccion in direcciones)
        {
            StartCoroutine(GenerarExplosión(posicion, direccion));
        }
    }

    IEnumerator GenerarExplosión(Vector2 posicion, Vector2 direccion)
    {
        for (int i = 1; i <= radioExplosion; i++) // Explosión con alcance de 'radioExplosion' bloques
        {
            Vector2 nuevaPos = posicion + direccion * i * tamañoCelda;

            // Instancia la explosión visual en la nueva posición
            Instantiate(explosionPrefab, nuevaPos, Quaternion.identity);

            // Verifica si hay un objeto destruible en la nueva posición
            Collider2D colision = Physics2D.OverlapCircle(nuevaPos, 0.2f);  // Asegúrate de que el tamaño de OverlapCircle sea adecuado
            if (colision != null && colision.CompareTag("Destruible"))
            {
                Destroy(colision.gameObject); // Destruye el objeto destruible
            }

            // Verifica si el jugador está en el rango de la explosión
            if (colision != null && colision.CompareTag("Jugador"))
            {
                // Aquí puedes aplicar la lógica para "matar" al jugador.
                // Por ejemplo, destruir al jugador o reducir su salud
                Destroy(colision.gameObject); // O cualquier otro método para eliminar al jugador
            }

            // Si encuentra un objeto destruible o al jugador, detiene la propagación en esa dirección
            if (colision != null && (colision.CompareTag("Destruible") || colision.CompareTag("Jugador")))
            {
                break;
            }

            yield return new WaitForSeconds(0.1f); // Pequeño delay entre cada instancia de explosión
        }
    }
}
