using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float tamañoCelda = 1f; // Tamaño de la cuadrícula

    void Start()
    {
        AlinearObjetos();
    }

    void AlinearObjetos()
    {
        foreach (Transform obj in transform)
        {
            Vector3 posAlineada = new Vector3(
                Mathf.Round(obj.position.x / tamañoCelda) * tamañoCelda,
                Mathf.Round(obj.position.y / tamañoCelda) * tamañoCelda,
                obj.position.z
            );
            obj.position = posAlineada;
        }
    }
}
