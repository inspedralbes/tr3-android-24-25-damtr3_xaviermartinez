using UnityEngine;
using UnityEngine.Tilemaps;

public class BloqueDestruible : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponentInParent<Tilemap>();
    }

    void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Explosion"))
        {
            Vector3Int posicionTile = tilemap.WorldToCell(transform.position);
            tilemap.SetTile(posicionTile, null); // Destruye solo este tile
            Destroy(gameObject); // Opcional: Elimina el GameObject si es necesario
        }
    }
}