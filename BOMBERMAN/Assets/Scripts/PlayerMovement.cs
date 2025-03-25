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

        // Verificar si hay un obstáculo (pared/bloque) o bomba
        return Physics2D.OverlapBox(posicionObjetivo, size, 0f, capaObstaculos | capaBomba);
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / gridSize) * gridSize + gridSize / 2;
        float y = Mathf.Floor(posicion.y / gridSize) * gridSize + gridSize / 2;
        return new Vector2(x, y);
    }

    // Método para colocar una bomba
    private void ColocarBomba(Vector2 posicion)
    {
        if (bombasColocadas < maxBombas)
        {
            Instantiate(bombaPrefab, posicion, Quaternion.identity);  // Instanciamos la bomba
            bombasColocadas++;  // Incrementamos el contador de bombas colocadas
        }
    }

    // Método para eliminar una bomba (cuando explota)
    public void EliminarBomba()
    {
        if (bombasColocadas > 0)
        {
            bombasColocadas--;  // Reducimos el contador de bombas colocadas
        }
    }

    // Método para reducir la salud del jugador
    public void RecibirDaño(int cantidad)
    {
        salud -= cantidad;
        if (salud <= 0)
        {
            MatarJugador();
        }
    }

    // Método para matar al jugador
    private void MatarJugador()
    {
        // Aquí puedes agregar una animación de muerte o cualquier otra lógica de muerte
        Destroy(gameObject);  // Destruye el jugador
    }

    // Método para aumentar el límite de bombas (cuando el jugador recoge el power-up)
    public void AumentarMaxBombas(int cantidad)
    {
        maxBombas += cantidad;
        Debug.Log("Max bombas aumentado: " + maxBombas);
    }

    // Método para reducir el límite de bombas (cuando el power-up desaparece)
    public void ReducirMaxBombas(int cantidad)
    {
        maxBombas -= cantidad;
    }

    // Método para aumentar la velocidad del jugador
    public void AumentarVelocidad(float cantidad)
    {
        velocidad += cantidad;
        Debug.Log("Velocidad aumentada: " + velocidad);
        StartCoroutine(DesactivarVelocidad(5f));  // Desactivamos después de 5 segundos
    }

    // Corutina para devolver la velocidad original después de un tiempo
    private IEnumerator DesactivarVelocidad(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        velocidad -= 2f;  // Volvemos a la velocidad original
        Debug.Log("Velocidad restaurada: " + velocidad);
    }

    // Método para reducir la velocidad del jugador
    public void ReducirVelocidad(float cantidad)
    {
        velocidad -= cantidad;
    }

    // Método que detecta la colisión con power-ups usando OnCollisionEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobamos si es un power-up de velocidad o bombas
        if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            Debug.Log("Power-up de velocidad detectado");
            AumentarVelocidad(2f);  // Aumentamos la velocidad
            Destroy(collision.gameObject);  // Destruimos el power-up
        }
        else if (collision.gameObject.CompareTag("BombaBoost"))
        {
            Debug.Log("Power-up de bomba detectado");
            AumentarMaxBombas(1);  // Aumentamos el límite de bombas
            Destroy(collision.gameObject);  // Destruimos el power-up
        }
    }
}
