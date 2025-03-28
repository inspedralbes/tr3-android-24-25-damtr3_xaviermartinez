using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float tamañoCelda = 1f;

    public void IniciarExplosion(Vector2 posicion, int radio, LayerMask capaObstaculos, LayerMask capaDestruible)
    {
        Instantiate(explosionPrefab, posicion, Quaternion.identity);
        Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in direcciones)
        {
            StartCoroutine(PropagarExplosion(posicion, dir, radio, capaObstaculos, capaDestruible));
        }
    }

    IEnumerator PropagarExplosion(Vector2 posicion, Vector2 direccion, int radio, LayerMask obstaculos, LayerMask destruible)
    {
        for (int i = 1; i <= radio; i++)
        {
            Vector2 nuevaPos = posicion + direccion * i * tamañoCelda;

            // Verificar obstáculos indestructibles
            if (Physics2D.OverlapCircle(nuevaPos, 0.1f, obstaculos))
            {
                break;
            }

            // Crear explosión
            Instantiate(explosionPrefab, nuevaPos, Quaternion.identity);

            // Verificar objetos destruibles
            Collider2D col = Physics2D.OverlapCircle(nuevaPos, 0.1f, destruible);
            if (col != null)
            {
                Destroy(col.gameObject);
                break; // Detener propagación si hay destruible
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}