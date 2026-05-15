using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 end;

    private Vector3 beforePosPlatform;
    private CharacterController playerController;
    private PlayerController playerCode;

    void LateUpdate() // Usamos LateUpdate para que la plataforma ya se haya movido
    {
        if (playerController != null)
        {
            // Calculamos cuánto se movió la plataforma en este frame
            Vector3 movimientoPlataforma = transform.position - beforePosPlatform;

            // Se lo sumamos "a la fuerza" al CharacterController
            if (!playerCode.isDashing) playerController.Move(movimientoPlataforma);
        }

        beforePosPlatform = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            playerCode = other.GetComponent<PlayerController>();
            playerController = other.GetComponent<CharacterController>();
            beforePosPlatform = transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            playerCode = null;
            playerController = null;
        }
    }
    void Start()
    {
        end = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(end.x, transform.position.y, end.z), speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, new Vector3(end.x, transform.position.y, end.z)) < 0.1f)
        {
            end = (end == pointA.position) ? pointB.position : pointA.position;
        }

        if (pointA == null || pointB == null) Debug.LogError("te falto la pocision inicial o final bobina");
    }

}
