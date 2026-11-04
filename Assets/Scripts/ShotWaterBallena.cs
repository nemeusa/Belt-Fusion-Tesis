using System.Collections;
using UnityEngine;

public class ShotWaterBallena : MonoBehaviour
{

    [SerializeField] AudioClip deathAudio;
    [SerializeField] float deathDuration = 0.2f;

    [SerializeField] float speedOutCamera = 10;

    [SerializeField, Range(0, 1)] float _icePlayerForceWater = 1;

    [Header("Impulso al activarse")]
    [SerializeField] float launchSpeed = 12f;     // velocidad vertical del impulso
    [SerializeField] float launchWindow = 0.5f;   // tiempo que espera al player después de activarse

    bool isFlyDeath;
    float launchTimer;

    Transform playerMesh;

    // Lo llama el géiser cuando empieza el chorro
    public void Launch() => launchTimer = launchWindow;

    private void LateUpdate()
    {
        if (isFlyDeath) playerMesh.position += Vector3.up * speedOutCamera * Time.deltaTime;
    }

    private void Update()
    {
        if (launchTimer > 0) launchTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (!player._fsm.WhatCurrentState(TypeFSM.Ice))
            {
                playerMesh = player.meshFather.transform;

                StartCoroutine(OutCamDeath());

                GameManager.instance.Death(collision.gameObject, deathDuration, deathAudio, null);
            }
            else if (launchTimer > 0)
            {
                // impulso único, aunque esté en el suelo
                player._playerVelocity.y = Mathf.Max(player._playerVelocity.y, launchSpeed);
                
                launchTimer = 0;
            }
            else
            {
                player._playerVelocity.y += _icePlayerForceWater;
            }
        }
    }

    IEnumerator OutCamDeath()
    {
        isFlyDeath = true;

        yield return new WaitForSeconds(deathDuration);

        isFlyDeath = false;

        playerMesh = null;
    }

}
