using UnityEngine;

public class DestroySomething : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpinBallEffect>() != null)
            Destroy(other);
    }
}
