using UnityEngine;

public class TrapAxe : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] float speed = 2f;      
    [SerializeField] float angleLimit = 90f; 

    void Update()
    {
        
        float angle = Mathf.Sin(Time.time * speed) * angleLimit;

        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
