using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Explosion : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float tamañoCelda = 1f;
    public int damage = 1; // Daño por explosión

    [Header("Configuración de Tilemap")]
    public Tilemap destructibleTilemap;
    public TileBase tileADestruir;

    void Start()
    {
        if (destructibleTilemap == null)
        {
            destructibleTilemap = GameObject.Find("DestruibleBlock").GetComponent<Tilemap>();
        }
    }

    public void IniciarExplosion(Vector2 posicion, int radio, LayerMask capaObstaculos, LayerMask capaDestruible)
    {
        // Explosión central
        GameObject explosionCentral = Instantiate(explosionPrefab, posicion, Quaternion.identity);
        Destroy(explosionCentral, 5f);

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
                break; // Detener si hay un bloque indestructible
            }

            // 2. Verificar y destruir tile destruible
            Vector3Int tilePos = destructibleTilemap.WorldToCell(nuevaPos);
            TileBase tile = destructibleTilemap.GetTile(tilePos);

            if (tile == tileADestruir)
            {
                destructibleTilemap.SetTile(tilePos, null); // Destruir el bloque
            }

            // 3. Crear explosión visual en esta posición
            Quaternion rotacion = ObtenerRotacionSegunDireccion(direccion);
            GameObject explosion = Instantiate(explosionPrefab, nuevaPos, rotacion);
            Destroy(explosion, 5f); // Destruir después de 5 segundos

            yield return new WaitForSeconds(0.1f);
        }
    }

    Quaternion ObtenerRotacionSegunDireccion(Vector2 direccion)
    {
        if (direccion == Vector2.up)
            return Quaternion.Euler(0, 0, 0);
        else if (direccion == Vector2.down)
            return Quaternion.Euler(0, 0, 180);
        else if (direccion == Vector2.left)
            return Quaternion.Euler(0, 0, 90);
        else
            return Quaternion.Euler(0, 0, -90);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si colisiona con un jugador
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.PerderVida(); // Reducir vida
            }
        }
    }
}
