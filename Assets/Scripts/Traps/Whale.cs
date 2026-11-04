using System.Collections;
using UnityEngine;

public class Whale : TimeTrap, ISlamReactive
{
    [SerializeField] private float tiempoDisparando = 2f;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float launchSpeed = 12f;

    [SerializeField] Animator waterAni;
    [SerializeField] ParticleSystem waterParticles;

    private bool isBusy;
    private bool isShooting;
    private float speedFactor = 1f;

    // Lo llama el IceState cuando el panzazo impacta cerca del géiser
    public void OnSlam(PlayerController player)
    {
        bool started = TryActivate();
        if (!started && !isShooting) return; // en cooldown no hay agua, no lo lanza

        player._playerVelocity.y = Mathf.Max(player._playerVelocity.y, launchSpeed);
        player.ForceGlide();
    }

    public bool TryActivate()
    {
        if (isBusy) return false;
        StartCoroutine(ShootRoutine());
        return true;
    }

    private IEnumerator ShootRoutine()
    {
        isBusy = true;
        isShooting = true;

        waterParticles.Play();
        waterAni.SetBool("Up", true);
        yield return Wait(0.5f + tiempoDisparando);

        waterAni.SetBool("Up", false);
        waterParticles.Stop();
        isShooting = false;
        yield return Wait(0.5f + cooldown);

        isBusy = false;
    }

    private WaitForSeconds Wait(float t) => new WaitForSeconds(t / Mathf.Max(speedFactor, 0.01f));

    protected override void SlowdownTrap(float factor)
    {
        speedFactor = factor;
        waterAni.speed = factor;
    }

    protected override void NormalizeTrap()
    {
        speedFactor = 1f;
        waterAni.speed = 1f;
    }
}
