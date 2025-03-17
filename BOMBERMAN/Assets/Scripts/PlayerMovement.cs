using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float gridSize = 1f;
    public LayerMask capaObstaculos;

    private Rigidbody2D rb;
    private Vector2 destino;
    private bool enMovimiento = false;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        destino = AlinearAPosicion(transform.position);
    }

    void Update()
    {
        if (!enMovimiento)
        {
            float movimientoX = Input.GetAxisRaw("Horizontal");
            float movimientoY = Input.GetAxisRaw("Vertical");

            // Solo permite moverse en una dirección a la vez
            if (movimientoX != 0)
            {
                movimientoY = 0;
            }

            Vector2 direccion = new Vector2(movimientoX, movimientoY);
            Vector2 nuevaPosicion = AlinearAPosicion((Vector2)transform.position + direccion * gridSize);

            // Verifica si hay un obstáculo antes de mover
            if (direccion != Vector2.zero && !HayObstaculo(nuevaPosicion))
            {
                destino = nuevaPosicion;
                enMovimiento = true;

                // Actualiza animación
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MoveX", movimientoX);
                animator.SetFloat("MoveY", movimientoY);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
    }

    void FixedUpdate()
    {
        if (enMovimiento)
        {
            transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.fixedDeltaTime);

            if ((Vector2)transform.position == destino)
            {
                enMovimiento = false;
                animator.SetBool("IsMoving", false);
            }
        }
    }

    private bool HayObstaculo(Vector2 posicionObjetivo)
    {
        Collider2D colision = Physics2D.OverlapCircle(posicionObjetivo, 0.1f, capaObstaculos);
        return colision != null;
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Round(posicion.x / gridSize) * gridSize;
        float y = Mathf.Round(posicion.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }
}
