using UnityEngine;

public class BloqueDestruible : MonoBehaviour
{
    // Este m�todo se llamar� cuando otro objeto entre en su colisi�n
    private void OnTriggerEnter2D(Collider2D colision)
    {
        // Comprueba si el objeto que colisiona tiene el tag "Explosion"
        if (colision.CompareTag("Explosion"))
        {
            // Destruye el bloque
            Destroy(gameObject);
        }
    }
}
