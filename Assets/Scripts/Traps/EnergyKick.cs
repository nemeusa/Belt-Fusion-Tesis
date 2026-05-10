using UnityEngine;

public class EnergyKick : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushDuration = 0.2f;

    [SerializeField] GameObject _shockPrefab;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if(player.isDashing) return;

            audioSource.Play();

            Vector3 pushDirection = -other.transform.forward;

            player.ApplyKnockback(pushDirection, pushForce, pushDuration);

            player.CountMoves(0);

            var d = Instantiate(_shockPrefab, player.transform.position, Quaternion.identity);

            Destroy(d, 2);

        }
    }
}
