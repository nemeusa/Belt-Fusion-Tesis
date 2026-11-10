using UnityEngine;

public class CloudAcid : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float leftLimit = -10f;   // posición X izquierda
    [SerializeField] private float rightLimit = 10f;   // posición X derecha

 
    private int direction = 1;


    private void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x >= rightLimit) direction = -1;
        else if (transform.position.x <= leftLimit) direction = 1;

    }
}
