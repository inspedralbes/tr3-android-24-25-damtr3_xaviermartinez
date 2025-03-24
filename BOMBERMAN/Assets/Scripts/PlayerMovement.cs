using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float gridSize = 1f;

    // Ajuste: Cambiar por el Layer específico "BloquesDestruibles"
    public LayerMask capaObstaculos; // Seleccionar desde Inspector el Layer 7 (BloquesDestruibles)

    private Vector2 destino;
    private bool enMovimiento = false;
    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Alinear personaje al centro de la cuadrícula al inicio
        destino = AlinearAPosicion(transform.position);
        transform.position = destino;
    }

    void Update()
    {
        if (!enMovimiento)
        {
            float movX = Input.GetAxisRaw("Horizontal");
            float movY = Input.GetAxisRaw("Vertical");

            // Permitir moverse solo en una dirección a la vez
            if (movX != 0) movY = 0;

            Vector2 direccion = new Vector2(movX, movY);
            Vector2 nuevaPosicion = AlinearAPosicion((Vector2)transform.position + direccion * gridSize);

            if (direccion != Vector2.zero && !HayObstaculo(nuevaPosicion, direccion))
            {
                destino = nuevaPosicion;
                enMovimiento = true;

                // Activar animaciones
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MoveX", direccion.x);
                animator.SetFloat("MoveY", direccion.y);
            }
        }
    }

    void FixedUpdate()
    {
        if (enMovimiento)
        {
            transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, destino) < 0.01f)
            {
                transform.position = destino;
                enMovimiento = false;
                animator.SetBool("IsMoving", false);
            }
        }
    }

    private bool HayObstaculo(Vector2 posicionObjetivo, Vector2 direccion)
    {
        Vector2 size = boxCollider.size * 0.9f;

        // BoxCast ajustado a la Layer "BloquesDestruibles" y al LayerMask desde el Inspector
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, size, 0f, direccion, gridSize, capaObstaculos);

        if (hit.collider != null)
        {
            Debug.Log("Colisión detectada con: " + hit.collider.name); // Para verificar colisión en consola
        }

        return hit.collider != null;
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / gridSize) * gridSize + gridSize / 2;
        float y = Mathf.Floor(posicion.y / gridSize) * gridSize + gridSize / 2;
        return new Vector2(x, y);
    }
}
