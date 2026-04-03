using UnityEngine;

public class RopeAnchor : MonoBehaviour
{
    public Rigidbody rbPuente; 
    public float fuerzaDeCaida = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FireBall>())
        {
            RomperPuente();
        }
    }

    void RomperPuente()
    {
        if (rbPuente != null)
        {
            rbPuente.isKinematic = false;

            rbPuente.AddTorque(transform.forward * fuerzaDeCaida, ForceMode.Impulse);

            Debug.Log("¡Cuerda cortada! El puente está cayendo.");
        }

        gameObject.SetActive(false);
    }
}
