using UnityEngine;

public class CheckpointEffectTrigger : MonoBehaviour
{
    [SerializeField] Transform playerEffectPoint;


    bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            var c = Instantiate(player.respawnEffectsPrefabs, playerEffectPoint.position, Quaternion.identity);
            Destroy(c, 2);
            activated = true;
        }
    }
}
