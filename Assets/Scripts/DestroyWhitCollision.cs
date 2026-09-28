using UnityEngine;

public class DestroyWhitCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject, 0.1f);
    }
}
