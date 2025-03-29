using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Explosion : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float tamañoCelda = 1f;

    [Header("Configuración de Tilemap")]
    public Tilemap destructibleTilemap; // Asignar en el Inspector
    public TileBase tileADestruir;      // Asignar el Tile específico

    void Start()
    {
        // Backup: Busca automáticamente si no está asignado
        if (destructibleTilemap == null)
        {
            destructibleTilemap = GameObject.Find("DestruibleBlock").GetComponent<Tilemap>();
        }
    }

    public void IniciarExplosion(Vector2 posicion, int radio, LayerMask capaObstaculos, LayerMask capaDestruible)
    {
        Instantiate(explosionPrefab, posicion, Quaternion.identity);
        Vector2[] direcciones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in direcciones)
        {
            StartCoroutine(PropagarExplosion(posicion, dir, radio, capaObstaculos));
        }
    }

    IEnumerator PropagarExplosion(Vector2 posicion, Vector2 direccion, int radio, LayerMask obstaculos)
    {
        for (int i = 1; i <= radio; i++)
        {
            Vector2 nuevaPos = posicion + direccion * i * tamañoCelda;

            // 1. Verificar obstáculos indestructibles
            if (Physics2D.OverlapCircle(nuevaPos, 0.1f, obstaculos))
            {
                break;
            }

            // 2. Verificar tiles destructibles
            Vector3Int tilePos = destructibleTilemap.WorldToCell(nuevaPos);
            TileBase tile = destructibleTilemap.GetTile(tilePos);

            if (tile == tileADestruir) // Si falla, usar if (tile != null)
            {
                destructibleTilemap.SetTile(tilePos, null);
                Instantiate(explosionPrefab, destructibleTilemap.GetCellCenterWorld(tilePos), Quaternion.identity);
                break;
            }

            Instantiate(explosionPrefab, nuevaPos, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
}