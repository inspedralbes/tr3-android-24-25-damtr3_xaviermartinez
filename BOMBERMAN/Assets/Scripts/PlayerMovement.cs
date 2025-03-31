using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement; // Para reiniciar la escena

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float gridSize = 1f;
    public LayerMask capaObstaculos;
    public LayerMask capaBomba;
    public LayerMask capaDestruible;
    public int salud = 100;
    public int vidas = 3;
    public TMP_Text textoVidas;
    public GameObject bombaPrefab;
    public int maxBombas = 1;
    public int bombasColocadas = 0;

    private float velocidadOriginal;
    private int maxBombasOriginal;
    public Coroutine velocidadCoroutine;
    public Coroutine bombasCoroutine;

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public KeyCode bombKey = KeyCode.Space;

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
        velocidadOriginal = velocidad;
        maxBombasOriginal = maxBombas;
        destino = AlinearAPosicion(transform.position);
        transform.position = destino;
        ActualizarVidasUI();
    }

    void Update()
    {
        if (!enMovimiento)
        {
            float movX = Input.GetAxisRaw(horizontalAxis);
            float movY = Input.GetAxisRaw(verticalAxis);

            if (movX != 0) movY = 0;
            Vector2 direccion = new Vector2(movX, movY);
            Vector2 nuevaPosicion = AlinearAPosicion((Vector2)transform.position + direccion * gridSize);

            if (direccion != Vector2.zero && !HayObstaculo(nuevaPosicion))
            {
                destino = nuevaPosicion;
                enMovimiento = true;
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MoveX", direccion.x);
                animator.SetFloat("MoveY", direccion.y);
            }
        }

        if (Input.GetKeyDown(bombKey) && bombasColocadas < maxBombas)
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
        return Physics2D.OverlapBox(posicionObjetivo, size, 0f, capaObstaculos | capaBomba | capaDestruible);
    }

    private Vector2 AlinearAPosicion(Vector2 posicion)
    {
        float x = Mathf.Floor(posicion.x / gridSize) * gridSize + gridSize / 2;
        float y = Mathf.Floor(posicion.y / gridSize) * gridSize + gridSize / 2;
        return new Vector2(x, y);
    }

    private void ColocarBomba(Vector2 posicion)
    {
        if (bombasColocadas < maxBombas)
        {
            GameObject bomba = Instantiate(bombaPrefab, posicion, Quaternion.identity);
            bombasColocadas++;
            BombaLogic bombaLogic = bomba.GetComponent<BombaLogic>();
            if (bombaLogic != null)
            {
                bombaLogic.SetPlayer(this);
            }
        }
    }

    public void EliminarBomba()
    {
        bombasColocadas = Mathf.Max(0, bombasColocadas - 1);
    }

    public void AplicarSpeedBoost(float multiplicador, float duracion)
    {
        if (velocidadCoroutine != null)
        {
            StopCoroutine(velocidadCoroutine);
        }
        velocidad = velocidadOriginal * multiplicador;
        velocidadCoroutine = StartCoroutine(RemoverSpeedBoost(duracion));
    }

    public void AplicarBombaBoost(int cantidad, float duracion)
    {
        if (bombasCoroutine != null)
        {
            StopCoroutine(bombasCoroutine);
        }
        maxBombas += cantidad;
        bombasCoroutine = StartCoroutine(RemoverBombaBoost(cantidad, duracion));
    }

    public IEnumerator RemoverSpeedBoost(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        velocidad = velocidadOriginal;
    }

    public IEnumerator RemoverBombaBoost(int cantidad, float duracion)
    {
        yield return new WaitForSeconds(duracion);
        maxBombas = Mathf.Max(maxBombasOriginal, maxBombas - cantidad);
    }

    public void RecibirDaño(int cantidad)
    {
        salud -= cantidad;
        if (salud <= 0)
        {
            PerderVida();
        }
    }

    public void PerderVida()
    {
        vidas--;
        salud = 100;
        ActualizarVidasUI();

        if (vidas <= 0)
        {
            GameOver();
        }
        else
        {
            ReiniciarNivel();
        }
    }

    private void ActualizarVidasUI()
    {
        if (textoVidas != null)
        {
            textoVidas.text = $"VIDAS: {vidas}";
        }
    }

    private void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        Debug.Log("¡GAME OVER!");
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            AplicarSpeedBoost(2f, 5f);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BombaBoost"))
        {
            AplicarBombaBoost(1, 5f);
            Destroy(other.gameObject);
        }
    }
}
