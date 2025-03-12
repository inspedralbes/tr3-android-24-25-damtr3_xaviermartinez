using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad del jugador

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del jugador
        animator = GetComponent<Animator>(); // Obtiene el Animator
    }

    void Update()
    {
        // Captura el input de las teclas de movimiento
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");

        // Normaliza el movimiento para evitar velocidad diagonal más rápida
        movimiento = new Vector2(movimientoX, movimientoY).normalized;

        // Enviar valores al Animator
        animator.SetFloat("MoveX", movimiento.x);
        animator.SetFloat("MoveY", movimiento.y);
        animator.SetBool("IsMoving", movimiento != Vector2.zero);
    }

    void FixedUpdate()
    {
        // Mueve al jugador en la dirección indicada
        rb.linearVelocity = movimiento * velocidad;
    }
}
