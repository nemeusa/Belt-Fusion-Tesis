using System.Collections;
using UnityEngine;

public class MeteoriteFire : MonoBehaviour
{
    [SerializeField] AudioClip deathAudio;
    [SerializeField] ParticleSystem deathEffects;
    [SerializeField] float deathDuration = 0.2f;

    [SerializeField] float speedOutCamera = 10;

    bool isFlyDeath;

    Transform playerMesh;

    PlayerController _player;

    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushDuration = 0.2f;

    [SerializeField] Transform shotPlayerPoint;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player._fsm.WhatCurrentState(TypeFSM.Fire))
            {
                player.meshFather.SetActive(false);
                _player = player;
                player.dontMovePlayer = true;
                player.dontChangeElement = true;
                player.dontDobleJump = true;

                player.OnJumpPressed += ShootPlayer;

                player.transform.position = shotPlayerPoint.position;

                player.transform.forward = Vector3.forward;

                player.jumpCount = 0;
                player.dashCount = 0;

            }

            else
            {
                playerMesh = player.meshFather.transform;

                StartCoroutine(OutCamDeath());
                GameManager.instance.Death(collision.gameObject, deathDuration, deathAudio, deathEffects);
            }
        }
    }

    void ShootPlayer()
    {

        GameManager.instance.PlaySound(deathAudio);

        _player.OnJumpPressed -= ShootPlayer;

        var a = Instantiate(deathEffects, _player.transform.position, deathEffects.transform.rotation);
        a.Play();
        Destroy(a, 1);

        _player.meshFather.SetActive(true);

        _player.dontMovePlayer = false;

        _player.isDeath = false;
        _player.dontChangeElement = false;

        _player.dontDobleJump = false;

        Vector3 pushDirection = _player.transform.forward;

        _player.ApplyKnockback(pushDirection, pushForce, pushDuration);


        _player = null;

    }

    private void LateUpdate()
    {
        if (isFlyDeath) playerMesh.position += Vector3.back * speedOutCamera * Time.deltaTime;
    }


    IEnumerator OutCamDeath()
    {
        isFlyDeath = true;

        yield return new WaitForSeconds(deathDuration);

        isFlyDeath = false;
    }

}
