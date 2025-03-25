using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public GameObject explosionPrefab;   // Prefab de la explosi�n
    public float tama�oCelda = 1f;       // Tama�o de la cuadr�cula
    public int radioExplosion = 3;       // Radio de bloques afectados por la explosi�n

    public void IniciarExplosi�n(Vector2 posicion)
    {
        // Instancia la explosi�n en el centro
        Instantiate(explosionPrefab, posicion, Quaternion.identity);

        // Explosi�n en las 4 direcciones (arriba, abajo, izquierda, derecha)
        Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 direccion in direcciones)
        {
            StartCoroutine(GenerarExplosi�n(posicion, direccion));
        }
    }

    IEnumerator GenerarExplosi�n(Vector2 posicion, Vector2 direccion)
    {
        for (int i = 1; i <= radioExplosion; i++) // Explosi�n con alcance de 'radioExplosion' bloques
        {
            Vector2 nuevaPos = posicion + direccion * i * tama�oCelda;

            // Instancia la explosi�n en la nueva posici�n
            GameObject explosion = Instantiate(explosionPrefab, nuevaPos, Quaternion.identity);

            // Verifica si hay un objeto destruible en la nueva posici�n
            Collider2D colision = Physics2D.OverlapCircle(nuevaPos, 0.2f);
            if (colision != null && colision.CompareTag("Destruible"))
            {
                Destroy(colision.gameObject); // Destruye el objeto destruible
                break; // Detiene la propagaci�n en esa direcci�n
            }

            yield return new WaitForSeconds(0.1f); // Peque�o delay entre cada instancia de explosi�n
        }
    }
}
