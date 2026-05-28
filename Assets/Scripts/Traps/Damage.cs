using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    [SerializeField] AudioClip deathAudio;
    [SerializeField] ParticleSystem deathEffects;
    [SerializeField] float deathDuration = 0.2f;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            GameManager.instance.Death(collision.gameObject, deathDuration, deathAudio, deathEffects);

            //SceneManager.LoadScene(actualSceneName);
        }
    }
}
