using UnityEngine;

public class ColocarBomba : MonoBehaviour
{
    public GameObject bombaPrefab;  // Prefab de la bomba
    public float tiempoDeVida = 3f; // Tiempo antes de explotar

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Colocar();
        }
    }

    void Colocar()
    {
        // Instancia la bomba en la posición del jugador
        GameObject bomba = Instantiate(bombaPrefab, transform.position, Quaternion.identity);

        // Destruye la bomba después de un tiempo
        Destroy(bomba, tiempoDeVida);
    }
}
