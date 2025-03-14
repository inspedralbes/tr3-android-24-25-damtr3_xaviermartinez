using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 direccion;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Captura la última tecla presionada y la asigna como dirección
        if (Input.GetKeyDown(KeyCode.W))
            direccion = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S))
            direccion = Vector2.down;
        if (Input.GetKeyDown(KeyCode.A))
            direccion = Vector2.left;
        if (Input.GetKeyDown(KeyCode.D))
            direccion = Vector2.right;

        // Si sueltas una tecla y no hay otra presionada, se detiene
        if (Input.GetKeyUp(KeyCode.W) && direccion == Vector2.up)
            direccion = Vector2.zero;
        if (Input.GetKeyUp(KeyCode.S) && direccion == Vector2.down)
            direccion = Vector2.zero;
        if (Input.GetKeyUp(KeyCode.A) && direccion == Vector2.left)
            direccion = Vector2.zero;
        if (Input.GetKeyUp(KeyCode.D) && direccion == Vector2.right)
            direccion = Vector2.zero;

        // Enviar valores al Animator
        animator.SetBool("IsMoving", direccion != Vector2.zero);
        animator.SetFloat("MoveX", direccion.x);
        animator.SetFloat("MoveY", direccion.y);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direccion * velocidad;
    }
}
