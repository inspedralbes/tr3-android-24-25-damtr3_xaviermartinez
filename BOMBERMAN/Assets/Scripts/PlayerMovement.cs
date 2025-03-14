using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtiene el Animator
    }

    void Update()
    {
        // Captura el input de las teclas de movimiento
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");

        // Prioriza el movimiento horizontal sobre el vertical
        if (movimientoX != 0)
            movimientoY = 0;

        // Normaliza el movimiento para evitar que la velocidad diagonal sea mayor
        movimiento = new Vector2(movimientoX, movimientoY).normalized;

        // Enviar valores al Animator
        animator.SetBool("IsMoving", movimiento.magnitude > 0);
        animator.SetFloat("MoveX", movimiento.x);
        animator.SetFloat("MoveY", movimiento.y);
    }

    void FixedUpdate()
    {
        // Mueve al jugador en la dirección indicada
        rb.linearVelocity = movimiento * velocidad;
    }
}
