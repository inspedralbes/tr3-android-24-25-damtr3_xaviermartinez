using UnityEngine;
using System.Collections;

public class BombaLogic : MonoBehaviour
{
    public float tiempoExplosion = 3f;
    public int radioExplosion = 3;
    public LayerMask capaObstaculos;
    public LayerMask capaDestruible;

    private PlayerMovement jugador;
    private Explosion _explosion;

    void Awake()
    {
        _explosion = GetComponent<Explosion>();
    }

    public void SetPlayer(PlayerMovement player)
    {
        jugador = player;
        StartCoroutine(Explotar());
    }

    IEnumerator Explotar()
    {
        yield return new WaitForSeconds(tiempoExplosion);
        _explosion.IniciarExplosion(transform.position, radioExplosion, capaObstaculos, capaDestruible);
        jugador.EliminarBomba();
        Destroy(gameObject);
    }
    public void IniciarExplosion(Vector2 posicion, int radio, LayerMask capaObstaculos, LayerMask capaDestruible)
{        jugador.EliminarBomba();
        Destroy(gameObject);
    }
}