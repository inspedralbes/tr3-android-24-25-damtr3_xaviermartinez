using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad del jugador

    private Rigidbody2D rb;
    private Vector2 movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del jugador
    }

    void Update()
    {
        // Captura el input de las teclas de movimiento
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");

        // Normaliza el movimiento para evitar velocidad diagonal m�s r�pida
        movimiento = new Vector2(movimientoX, movimientoY).normalized;
    }

    void FixedUpdate()
    {
        // Mueve al jugador en la direcci�n indicada
        rb.linearVelocity = movimiento * velocidad;
    }
}
