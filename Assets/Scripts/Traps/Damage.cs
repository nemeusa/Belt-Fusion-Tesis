using UnityEngine;

public class Damage : MonoBehaviour
{

    [SerializeField] AudioClip deathAudio;
    [SerializeField] ParticleSystem deathEffects;
    [SerializeField] float deathDuration = 0.2f;

  
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
                GameManager.instance.Death(collision.gameObject, deathDuration, deathAudio, deathEffects);

        }
    }

}
