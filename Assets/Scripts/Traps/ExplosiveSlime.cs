using UnityEngine;

using System.Collections;

public class ExplosiveSlime : MonoBehaviour
{
    [Header("Activación")]
    [SerializeField] float fuseTime = 1.5f; // segundos entre que te acercás y explota

    [Header("Pulso del shader")]
    [SerializeField] Renderer slimeRenderer;
    [SerializeField] string pulseSpeedProperty = "_PulseSpeed"; // el Reference del shader, NO el nombre que se ve
    [SerializeField] float armedPulseSpeed = 12f;                // velocidad de latido justo antes de explotar

    [Header("Explosión")]
    [SerializeField] Transform explosionCenter;  // si lo dejás vacío usa este objeto
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] LayerMask hitMask = ~0;
    [SerializeField] GameObject explosionParticles; // acá van tus 2 partículas
    [SerializeField] float destroyDelay = 3f;             // tiempo para que terminen las partículas

    [Header("Death")]
    [SerializeField] AudioClip deathAudio;
    [SerializeField] ParticleSystem deathEffects;
    [SerializeField] float deathDuration = 0.2f;

    Material _mat;
    int _pulseId;
    float _basePulse;
    bool _armed;
    bool _exploded;
    Coroutine _fuseRoutine;



    void Awake()
    {
        if (slimeRenderer == null) slimeRenderer = GetComponentInChildren<Renderer>();

        _pulseId = Shader.PropertyToID(pulseSpeedProperty);
        _mat = slimeRenderer.materials[1]; // instancia propia, así no cambia a los demás slimes

        if (_mat.HasProperty(_pulseId))
            _basePulse = _mat.GetFloat(_pulseId);
        else
            Debug.LogWarning($"[ExplosiveSlime] El material no tiene la propiedad '{pulseSpeedProperty}'. Revisá el Reference en Shader Graph.", this);
    }

    void Update()
    {
        // Mismo patrón que tus otras trampas: al respawnear, vuelve a estar lista
        if (CheckpointManager.Instance == null) return;

        if (CheckpointManager.Instance.respawn && (_armed || _exploded))
            ResetSlime();
    }


    void OnTriggerEnter(Collider other)
    {
        if (_armed) return;

        var player = other.GetComponentInParent<PlayerController>();
        if (player == null || player.isDeath) return;

        _armed = true;
        StartCoroutine(Fuse());
    }

    IEnumerator Fuse()
    {
        float t = 0f;

        while (t < fuseTime)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fuseTime);

            // k*k hace que al principio lata casi normal y se acelere cada vez más
            SetPulse(Mathf.Lerp(_basePulse, armedPulseSpeed, k * k));
            yield return null;
        }

        _fuseRoutine = null;
        Explode();
    }

    void Explode()
    {
        Instantiate(explosionParticles, transform.position, Quaternion.identity);

        // Área de explosión: si el jugador está adentro, muere
        Vector3 center = explosionCenter != null ? explosionCenter.position : transform.position;
        var hits = Physics.OverlapSphere(center, explosionRadius, hitMask, QueryTriggerInteraction.Ignore);

        foreach (var h in hits)
        {
            var player = h.GetComponentInParent<PlayerController>();
            if (player != null && !player.isDeath)
            {
                GameManager.instance.Death(player.gameObject, deathDuration, deathAudio, deathEffects);
                break;
            }
        }

        // Ocultamos el slime y dejamos vivo el objeto hasta que terminen las partículas
        slimeRenderer.enabled = false;
        foreach (var c in GetComponents<Collider>()) c.enabled = false;
        //Destroy(gameObject, destroyDelay);
    }

    void ResetSlime()
    {
        // Por si el jugador murió por otra cosa en medio de la mecha
        if (_fuseRoutine != null)
        {
            StopCoroutine(_fuseRoutine);
            _fuseRoutine = null;
        }


        SetPulse(_basePulse);
        slimeRenderer.enabled = true;
        foreach (var c in GetComponents<Collider>()) c.enabled = true;

        _armed = false;
        _exploded = false;
    }

    void SetPulse(float value)
    {
        if (_mat != null && _mat.HasProperty(_pulseId)) _mat.SetFloat(_pulseId, value);
    }

    //void OnDestroy()
    //{
    //    if (_mat != null) Destroy(_mat);
    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.1f, 0.9f);
        Vector3 center = explosionCenter != null ? explosionCenter.position : transform.position;
        Gizmos.DrawWireSphere(center, explosionRadius);
    }
}
