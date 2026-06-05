using System.Collections;
using UnityEngine;

public class Ballena : MonoBehaviour
{
    [SerializeField] ParticleSystem _waterParticles;
    [SerializeField] float _cooldownShot = 2;

    [SerializeField] Collider hitBoxWater;

    private void Start()
    {
        hitBoxWater.enabled = false;

        StartCoroutine(WaterCooldown());
    }

    IEnumerator WaterCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(_cooldownShot);
            _waterParticles.Play();
            yield return new WaitForSeconds(0.1f);
            hitBoxWater.enabled = true;
            yield return new WaitForSeconds(_cooldownShot);
            hitBoxWater.enabled = false;
            _waterParticles.Stop();

        }
    }
}
