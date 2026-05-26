using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnergyKick : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushDuration = 0.2f;

    [SerializeField] GameObject _shockPrefab;

    AudioSource audioSource;

    Vignette vignette;
    ChromaticAberration aberration;
    LensDistortion distortion;


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
                if (player.globalVolume.profile.TryGet<Vignette>(out var vignetteTmp))
                    vignette = vignetteTmp;

                if (player.globalVolume.profile.TryGet<ChromaticAberration>(out var aberrationTmp)) aberration = aberrationTmp;

                if (player.globalVolume.profile.TryGet<LensDistortion>(out var distortionTmp)) distortion = distortionTmp;


                StartCoroutine(CongelaFrame());
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


    IEnumerator CongelaFrame()
    {
        StartCoroutine(EffectsCongelation());
        //Time.timeScale = 0.01f;
        //yield return new WaitForSeconds(0.002f);
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1;

    }

    IEnumerator EffectsCongelation()
    {
        // 1. Seteamos los valores iniciales del efecto
        vignette.intensity.value = 0.2f;
        vignette.color.value = Color.yellow;
        vignette.rounded.value = true;
        vignette.smoothness.value = 0.5f;
        aberration.intensity.value = 1;
        distortion.intensity.value = -0.5f;

        // 2. Esperamos el tiempo que el efecto está "al máximo"
        yield return new WaitForSeconds(1f);

        // 3. Transición suave (Fade Out)
        float duration = 1.0f; // Duración del suavizado en segundos
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Interpolamos los valores de los efectos hacia su estado original (0)
            distortion.intensity.value = Mathf.Lerp(-0.5f, 0f, t);
            aberration.intensity.value = Mathf.Lerp(1f, 0f, t);
            vignette.intensity.value = Mathf.Lerp(0.2f, 0f, t);
            vignette.smoothness.value = Mathf.Lerp(0.5f, 1f, t);

            yield return null; // Espera al siguiente frame
        }

        // 4. Aseguramos los valores finales y limpieza
        distortion.intensity.value = 0;
        aberration.intensity.value = 0;
        vignette.intensity.value = 0f;
        vignette.rounded.value = false;

        //vignette.intensity.value = 0.2f;
        //vignette.color.value = Color.yellow;
        //vignette.rounded.value = true;
        //vignette.smoothness.value = 0.5f;
        //aberration.intensity.value = 1;
        //distortion.intensity.value = -0.5f;

        //yield return new WaitForSeconds(1f);
        //distortion.intensity.value = 0;
        //aberration.intensity.value = 0;
        //vignette.intensity.value = 0f;
        //vignette.rounded.value = false;
        //vignette.smoothness.value = 1f;
    }
}
