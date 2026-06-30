using UnityEngine;

public class TrapResbaladiza : MonoBehaviour
{

    [Header("Configuración del Hielo")]
    [SerializeField] private float friccionHielo = 2f;  // Mientras más bajo, más resbala

    private float friccionOriginal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            // Guardamos la fricción que tenía el player por si en un futuro la cambiás
            friccionOriginal = player.agarreDelPiso;

            // Le ponemos el suelo patinoso
            player.agarreDelPiso = friccionHielo;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {

            // Le devolvemos el agarre normal cuando sale de la trampa
            player.agarreDelPiso = friccionOriginal;

        }
    }
}
