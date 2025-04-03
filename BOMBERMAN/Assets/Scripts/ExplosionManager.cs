using UnityEngine;

public class ExplosionManager : MonoBehaviour
{

    [Header("Configuraci�n")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private string playerTag = "Player";
    private bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("colision");
        // Verificar si el objeto que entr� en el trigger es el jugador
        if (other.CompareTag(playerTag))
        {
            // Obtener el componente PlayerHealth (o el que tenga el m�todo PerderVida)
            PlayerMovement vida = other.GetComponent<PlayerMovement>();

            if (vida != null && canDamage)
            {
                // Hacer que el jugador pierda vida
                vida.PerderVida(damageAmount);

                // Opcional: desactivar la bomba despu�s del impacto
                canDamage = false;

                // Opcional: reproducir efecto de explosi�n aqu� si no lo haces en otro lugar
            }
            else
            {
                Debug.LogWarning("El objeto con tag Player no tiene un componente con el m�todo PerderVida");
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Tocada una explosion");
        }
    }

}
