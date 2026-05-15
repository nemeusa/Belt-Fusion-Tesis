using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnergyKick : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushDuration = 0.2f;

    [SerializeField] GameObject _shockPrefab;

    AudioSource audioSource;



    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if(player.isDashing)
            {
                if (player.globalVolume.profile.TryGet<Vignette>(out var vignette))
                    StartCoroutine(CongelaFrame(vignette));

                return;
            }




            audioSource.Play();

            Vector3 pushDirection = -other.transform.forward;

            player.ApplyKnockback(pushDirection, pushForce, pushDuration);

            player.CountMoves(0);

            var d = Instantiate(_shockPrefab, player.transform.position, Quaternion.identity);

            Destroy(d, 2);

        }
    }


    IEnumerator CongelaFrame(Vignette vignette)
    {
        vignette.intensity.value = 0.5f;
        vignette.color.value = Color.yellow;
        vignette.rounded.value = true;
        vignette.smoothness.value = 0.5f;
        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(0.002f);
        Time.timeScale = 1;
        vignette.intensity.value = 0f;
        vignette.rounded.value = false;
        vignette.smoothness.value = 1f;
    }
}
