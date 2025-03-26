using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float gridSize = 1f;
    public LayerMask capaObstaculos;  // Obstáculos como paredes o bloques
    public LayerMask capaBomba;       // Capa para bombas ya colocadas
    public int salud = 100;           // Salud del jugador

    // Variables para las bombas
    public GameObject bombaPrefab;      // Prefab de la bomba
    public int maxBombas = 1;           // Número máximo de bombas que el jugador puede colocar
    private int bombasColocadas = 0;    // Contador de las bombas colocadas

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

        destino = AlinearAPosicion(transform.position);
        transform.position = destino;  // Alinear al inicio
    }

    void Update()
    {
        if (!enMovimiento)
        {
            float movX = Input.GetAxisRaw("Horizontal");
            float movY = Input.GetAxisRaw("Vertical");

            if (movX != 0) movY = 0;  // Una dirección a la vez
            Vector2 direccion = new Vector2(movX, movY);
            Vector2 nuevaPosicion = AlinearAPosicion((Vector2)transform.position + direccion * gridSize);

            if (direccion != Vector2.zero && !HayObstaculo(nuevaPosicion))
            {
                destino = nuevaPosicion;
                enMovimiento = true;

                // Activar animaciones
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MoveX", direccion.x);
                animator.SetFloat("MoveY", direccion.y);
            }
        }

        // Detectar la acción de colocar bomba
        if (Input.GetKeyDown(KeyCode.Space) && bombasColocadas < maxBombas)
        {
            ColocarBomba(transform.position);
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

    private bool HayObstaculo(Vector2 posicionObjetivo)
    {
        Vector2 size = boxCollider.size * 0.9f;
        return Physics2D.OverlapBox(posicionObjetivo, size, 0f, capaObstaculos | capaBomba);
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / gridSize) * gridSize + gridSize / 2;
        float y = Mathf.Floor(posicion.y / gridSize) * gridSize + gridSize / 2;
        return new Vector2(x, y);
    }

    private void ColocarBomba(Vector2 posicion)
    {
        Instantiate(bombaPrefab, posicion, Quaternion.identity);
        bombasColocadas++;
    }

    public void EliminarBomba()
    {
        if (bombasColocadas > 0) bombasColocadas--;
    }

    public void RecibirDaño(int cantidad)
    {
        salud -= cantidad;
        if (salud <= 0) MatarJugador();
    }

    private void MatarJugador()
    {
        Destroy(gameObject);
    }

    public void AumentarMaxBombas(int cantidad)
    {
        maxBombas += cantidad;
        Debug.Log("Max bombas aumentado: " + maxBombas);
    }

    public void ReducirMaxBombas(int cantidad)
    {
        maxBombas -= cantidad;
    }

    public void AumentarVelocidad(float cantidad)
    {
        velocidad += cantidad;
        StartCoroutine(DesactivarVelocidad(5f));
    }

    private IEnumerator DesactivarVelocidad(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        velocidad -= 2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            AumentarVelocidad(2f);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("BombaBoost"))
        {
            AumentarMaxBombas(1);
            Destroy(collision.gameObject);
        }
    }
}
