using System.Collections;
using UnityEngine;

public class TrapAxe : MonoBehaviour
{

    [Header("Configuración de Movimiento")]
    [SerializeField] float speed = 2f;
    [SerializeField] float angleLimit = 90f;
    public float pauseDuration = 1f;

    AudioSource audioSourse;

    private float currentSpeed;

    private void Awake()
    {
        audioSourse = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentSpeed = speed;
        // Iniciamos la corrutina una sola vez
        StartCoroutine(AxeRoutine());
    }

    IEnumerator AxeRoutine()
    {
        while (true)
        {
            // 1. Ir hacia la derecha (angleLimit)
            yield return StartCoroutine(RotateToAngle(angleLimit));

            // 2. Pausa en el extremo
            //Debug.Log("Frenando en derecha...");
            yield return new WaitForSeconds(pauseDuration);

            // 3. Ir hacia la izquierda (-angleLimit)
            yield return StartCoroutine(RotateToAngle(-angleLimit));

            // 4. Pausa en el otro extremo
            //Debug.Log("Frenando en izquierda...");
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    IEnumerator RotateToAngle(float targetAngle)
    {
        if (!GameManager.instance.player.isDeath)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            // Mientras no estemos lo suficientemente cerca del ángulo objetivo
            while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
            {
                if (!GameManager.instance.player.isDeath)
                {

                    transform.localRotation = Quaternion.RotateTowards(
                    transform.localRotation,
                    targetRotation,
                    currentSpeed * Time.deltaTime * 50f // Ajustá el multiplicador para la velocidad
                    );
                }

                // IMPORTANTE: Esto le dice a Unity "pará acá y seguí en el siguiente frame"
                // Sin esto, Unity explota.
                yield return null;
            }

            // Aseguramos que quede exacto al final
            transform.localRotation = targetRotation;
            audioSourse.Play();
        }

    
    }

    private void RalentizarTrampa(float factor) => currentSpeed = speed * factor;
    
    private void NormalizarTrampa() => currentSpeed = speed;
   

    private void OnEnable()
    {
        TimeSlow.OnTimeSlowed += RalentizarTrampa;
        TimeSlow.OnTimeNormalized += NormalizarTrampa;
    }

    private void OnDisable()
    {
        TimeSlow.OnTimeSlowed -= RalentizarTrampa;
        TimeSlow.OnTimeNormalized -= NormalizarTrampa;
    }
}
