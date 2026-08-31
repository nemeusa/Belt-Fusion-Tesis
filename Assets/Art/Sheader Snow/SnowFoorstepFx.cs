using UnityEngine;

public class SnowFootstepFX : MonoBehaviour
{
    [Header("Referencias")]
    public ParticleSystem snowParticles;
    public ParticleSystem dustParticles;
    public CharacterController playerController; // arrastrar el Player acá

    [Header("Ajustes - Nieve")]
    public float snowStepDistance = 1.2f;
    public int snowParticlesPerStep = 8;

    [Header("Ajustes - Polvo")]
    public float dustStepDistance = 1.2f;
    public int dustParticlesPerStep = 4;

    Vector3 lastStepPos;
    bool isOnSnow = false;

    void Start()
    {
        lastStepPos = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snow"))
            isOnSnow = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Snow"))
            isOnSnow = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Snow"))
            isOnSnow = false;
    }

    void Update()
    {
        // Si no estamos tocando el piso, no emitimos nada
        if (playerController != null && !playerController.isGrounded)
        {
            lastStepPos = transform.position; // igual actualizamos, para no acumular distancia falsa mientras está en el aire
            return;
        }

        float distanceMoved = Vector3.Distance(transform.position, lastStepPos);
        float threshold = isOnSnow ? snowStepDistance : dustStepDistance;

        if (distanceMoved >= threshold)
        {
            if (isOnSnow && snowParticles != null)
            {
                snowParticles.Emit(snowParticlesPerStep);
            }
            else if (!isOnSnow && dustParticles != null)
            {
                dustParticles.Emit(dustParticlesPerStep);
            }

            lastStepPos = transform.position;
        }
    }
}