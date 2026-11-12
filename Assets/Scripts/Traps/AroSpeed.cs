using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AroSpeed : MonoBehaviour
{
    [SerializeField] float multiplicatorSpeed = 1.5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.speed *= multiplicatorSpeed;
        }
    }
}
