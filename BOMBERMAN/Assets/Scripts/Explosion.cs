using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Explosion : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float tamañoCelda = 1f;
    public int damage = 1;

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

            if (Physics2D.OverlapCircle(nuevaPos, 0.1f, obstaculos))
            {
                break;
            }

            Vector3Int tilePos = destructibleTilemap.WorldToCell(nuevaPos);
            TileBase tile = destructibleTilemap.GetTile(tilePos);

            if (tile == tileADestruir)
            {
                destructibleTilemap.SetTile(tilePos, null);
            }

            Quaternion rotacion = ObtenerRotacionSegunDireccion(direccion);
            GameObject explosion = Instantiate(explosionPrefab, nuevaPos, rotacion);
            Destroy(explosion, 5f);

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
        Debug.Log($"Colisión detectada con: {other.gameObject.name} (Tag: {other.tag})");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado en explosión");
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.MuerteInstantanea();
            }
            else
            {
                Debug.LogError("Componente PlayerMovement no encontrado");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}