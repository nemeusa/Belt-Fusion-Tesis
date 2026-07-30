using UnityEngine;

public class SnowTriggerTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snow"))
        {
            Debug.Log("Entró a la nieve");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Snow"))
        {
            Debug.Log("Pisando nieve...");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Snow"))
        {
            Debug.Log("Salió de la nieve");
        }
    }
}