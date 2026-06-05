using System.Collections;
using UnityEngine;

public class ShotWaterBallena : MonoBehaviour
{


    [SerializeField] AudioClip deathAudio;
    [SerializeField] float deathDuration = 0.2f;


    [SerializeField] float speedOutCamera = 10;

    [SerializeField, Range(0, 1)] float _icePlayerForceWater = 1;

    bool isFlyDeath;

    Transform playerMesh;

    private void LateUpdate()
    {
        if (isFlyDeath) playerMesh.position += Vector3.up * speedOutCamera * Time.deltaTime;
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

            else player._playerVelocity.y += _icePlayerForceWater;

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
