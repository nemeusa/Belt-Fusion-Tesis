using UnityEngine;

/// <summary>
/// Slime: al entrar el jugador queda pegado y va más lento.
/// Para salir puede caminar fuera del slime (lento) o apretar salto
/// varias veces para despegarse y pegar un saltito.
/// Ponelo en un objeto con un Collider marcado como "Is Trigger".
/// </summary>
[RequireComponent(typeof(Collider))]
public class SlimeTrap : MonoBehaviour
{
    [Header("Efecto")]
    [SerializeField, Range(0.05f, 1f)] float slowMultiplier = 0.25f; // 0.25 = 25% de la velocidad normal
    [SerializeField] int pressesToEscape = 5;                        // veces que hay que apretar salto
    [SerializeField] float escapeHopForce = 6f;                      // impulso hacia arriba al despegarse

    [Header("Audio (opcional)")]
    [SerializeField] AudioClip stickSound;
    [SerializeField] AudioClip struggleSound;
    [SerializeField] AudioClip escapeSound;

    PlayerController _player;
    int _presses;


    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player == null || player.isDeath) return;

        _player = player;
        _presses = 0;
        player.StickToSlime(this, slowMultiplier);

        if (stickSound != null) player.audioSource.PlayOneShot(stickSound);
    }

    void OnTriggerExit(Collider other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player == null || player != _player) return;

        Release();
    }

    // Lo llama el PlayerController cada vez que apretás salto estando pegado
    public void Struggle(PlayerController player)
    {
        _presses++;

        if (struggleSound != null) player.audioSource.PlayOneShot(struggleSound);
        player.VibrarControl(0.1f, 0.2f, 0.08f);

        if (_presses >= pressesToEscape)
        {
            player._playerVelocity.y = escapeHopForce;
            if (escapeSound != null) player.audioSource.PlayOneShot(escapeSound);
            Release();
        }
    }

    void Release()
    {
        if (_player != null && _player.currentSlime == this) _player.ReleaseFromSlime();
        _player = null;
    }

    // Si el slime se desactiva o destruye con el player adentro, no lo dejamos trabado
    void OnDisable()
    {
        Release();
    }
}
