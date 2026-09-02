using UnityEngine;

public class FootStepParticles : MonoBehaviour
{

    [Header("Efectos de Partículas por Superficie")]
    [SerializeField] private ParticleSystem particulasTierra;
    [SerializeField] private ParticleSystem particulasHielo;
    [SerializeField] private ParticleSystem particulasPiedra;
    [SerializeField] private ParticleSystem particulasDefault;

    [Header("Configuración de Detección")]
    [SerializeField] private float distanciaRaycast = 0.5f;
    [SerializeField] private LayerMask capaSuelo;

    public PlayerController playerCode;

    private void Update()
    {
        if (playerCode != null) if (playerCode.moveInput.magnitude > playerCode.driftMagnitude) GenerarParticula();
    }

    private void GenerarParticula()
    {
        // Lanzamos un Raycast desde el pie hacia abajo para detectar qué pisamos
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, distanciaRaycast, capaSuelo))
        {
            ParticleSystem particulaAInstanciar = SeleccionarParticulaPorTag(hit.collider.tag);

            if (particulaAInstanciar != null)
            {
                // Instanciamos el sistema de partículas en el punto de contacto con el suelo
                ParticleSystem p = Instantiate(particulaAInstanciar, hit.point, Quaternion.LookRotation(hit.normal));
                p.Play();
                Destroy(p.gameObject, p.main.duration + p.main.startLifetime.constantMax);
            }
        }
    }

    private ParticleSystem SeleccionarParticulaPorTag(string tagSuelo)
    {
        switch (tagSuelo)
        {
            case "Ice":
            case "Snow":
                return particulasHielo;

            case "Dirt":
            case "Grass":
                return particulasTierra;

            case "Stone":
            case "Rock":
                return particulasPiedra;

            default:
                return particulasDefault;
        }
    }
}
