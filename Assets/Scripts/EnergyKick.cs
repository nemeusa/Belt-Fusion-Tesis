using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnergyKick : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushDuration = 0.2f;

    [SerializeField] GameObject _shockPrefab;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if(player.isDashing) return;

            Vector3 pushDirection = -other.transform.forward;

            player.ApplyKnockback(pushDirection, pushForce, pushDuration);

            var d = Instantiate(_shockPrefab, player.transform.position, Quaternion.identity);

            Destroy(d, 2);

        }
    }
}
