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
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Captura el input de las teclas de movimiento
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");

        // 🔥 **Evita combinaciones diagonales (prioriza última tecla presionada)**
        if (movimientoX != 0)
            movimientoY = 0;  // Si se mueve en X, cancela Y
        else if (movimientoY != 0)
            movimientoX = 0;  // Si se mueve en Y, cancela X

        // Guarda la dirección de movimiento
        movimiento = new Vector2(movimientoX, movimientoY);

        // Si se mueve, actualiza animación correctamente
        if (movimiento != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", movimiento.x);
            animator.SetFloat("MoveY", movimiento.y);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movimiento * velocidad;
    }
}
