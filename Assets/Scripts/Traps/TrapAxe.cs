using System.Collections;
using UnityEngine;

public class TrapAxe : TimeTrap
{
    [Header("Configuración de Movimiento")]
    [SerializeField] float speed = 2f;
    [SerializeField] float angleLimit = 90f;
    [SerializeField] float pauseDuration = 1f;
    [SerializeField] bool invertMove;

    AudioSource audioSourse;

    private float currentSpeed;
    private float currentPause;

    void Start()
    {
        audioSourse = GetComponent<AudioSource>();
        currentSpeed = speed;
        currentPause = pauseDuration;
        StartCoroutine(AxeRoutine());
    }

    IEnumerator AxeRoutine()
    {
        while (true)
        {
            if (!invertMove) yield return StartCoroutine(RotateToAngle(angleLimit));
            else yield return StartCoroutine(RotateToAngle(-angleLimit));


            yield return new WaitForSeconds(currentPause);

            if (!invertMove) yield return StartCoroutine(RotateToAngle(-angleLimit));
            else yield return StartCoroutine(RotateToAngle(angleLimit));

            yield return new WaitForSeconds(currentPause);
        }
    }

    IEnumerator RotateToAngle(float targetAngle)
    {
        if (!GameManager.instance.player.isDeath)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
            {
                if (!GameManager.instance.player.isDeath)
                {

                    transform.localRotation = Quaternion.RotateTowards(
                    transform.localRotation,
                    targetRotation,
                    currentSpeed * Time.deltaTime * 50f 
                    );
                }
                yield return null;
            }

            transform.localRotation = targetRotation;
            audioSourse.Play();
        }
    }

    protected override void SlowdownTrap(float factor)
    {
        currentSpeed = speed * factor;
        //currentPause = pauseDuration * factor;
    }
    protected override void NormalizeTrap()
    {
        currentSpeed = speed;
        //currentPause = pauseDuration;
    }
}
