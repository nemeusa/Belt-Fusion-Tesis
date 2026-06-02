using UnityEngine;

public class CheckpointEffectTrigger : MonoBehaviour
{
    public Transform playerEffectPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            var c = Instantiate(player.respawnEffectsPrefabs, playerEffectPoint.position, Quaternion.identity);

            Destroy(c, 2);
        }
    }
}
