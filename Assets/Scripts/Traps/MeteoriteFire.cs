using System.Collections;
using Unity.Cinemachine;
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
    [SerializeField, Range(0, 0.2f)] float pushForceUp = 0.05f;
    [SerializeField] float pushDuration = 0.2f;

    [SerializeField] Transform shotPlayerPoint;
    [SerializeField] float touchPower = 5;
    
    [SerializeField] float speedFarCam = 0.15f;
    int inputCurrentPressTimes;

    CinemachineFollow camFollowPlayer;

    [SerializeField] private MeshRenderer meshRenderer;

    private Material segundoMaterial;

    [SerializeField] GameObject shootEffects;
    [SerializeField] Transform shootPositionEffects;

    float countFresnel = 0;

    bool puede;

    private void Start()
    {
        segundoMaterial = meshRenderer.materials[1];
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player._fsm.WhatCurrentState(TypeFSM.Fire))
            {
                inputCurrentPressTimes = 0;
                player.xButtomRepeat.SetActive(true);
                player.meshFather.SetActive(false);
                _player = player;
                player.dontMovePlayer = true;
                player.dontChangeElement = true;
                player.dontDobleJump = true;

                player.meteoriteCamTarget.Priority = 30;
                camFollowPlayer = player.meteoriteCamTarget.GetComponent<CinemachineFollow>();

                player.OnJumpPressed += CountTouchInput;

                player.transform.position = shotPlayerPoint.position;

                player.transform.forward = Vector3.forward;

                player.jumpCount = 0;
                player.dashCount = 0;
                player._playerVelocity.y = -2f;

                player.isIntoMeteorite = true;

                StartCoroutine(CamFar());
            }

            else
            {
                playerMesh = player.meshFather.transform;

                StartCoroutine(OutCamDeath());
                GameManager.instance.Death(collision.gameObject, deathDuration, deathAudio, deathEffects);
            }
        }
    }

    void CountTouchInput()
    {
        //inputCurrentPressTimes++;
        camFollowPlayer.FollowOffset.z += touchPower;
        countFresnel -= 0.5f;

        //if (inputCurrentPressTimes >= inputMaxPressTimes) 
        if (camFollowPlayer.FollowOffset.z > -3) 
            ShootPlayer();
    }

    void ShootPlayer()
    {
        if (shootEffects != null)
        {
            var p = Instantiate(shootEffects, shootPositionEffects.position, Quaternion.identity);
            Destroy(p.gameObject, 2);
        }
      
        inputCurrentPressTimes = 0;

        _player.xButtomRepeat.SetActive(false);

        GameManager.instance.PlaySound(deathAudio);



        var a = Instantiate(deathEffects, _player.transform.position, deathEffects.transform.rotation);
        a.Play();
        Destroy(a, 1);

        _player.meshFather.SetActive(true);

        _player.dontMovePlayer = false;

        _player.isDeath = false;
        _player.dontChangeElement = false;

        _player.dontDobleJump = false;

        Vector3 pushDirection = _player.transform.forward + new Vector3(0, pushForceUp, 0);

        _player.ApplyKnockback(pushDirection, pushForce, pushDuration);

        _player.isIntoMeteorite = false;

        _player.meteoriteCamTarget.GetComponent<CinemachineFollow>().FollowOffset.z = -18.12f;

        _player.meteoriteCamTarget.Priority = 5;


        puede = false;
        _player.OnJumpPressed -= CountTouchInput;
        _player = null;

    }

    private void LateUpdate()
    {
        if (isFlyDeath) playerMesh.position += Vector3.back * speedOutCamera * Time.deltaTime;


    }


    IEnumerator CamFar()
    {
        float fres = 0;

        puede = true;
        //while (_player.isIntoMeteorite)
        while (puede)
        {
            //camFollowPlayer.FollowOffset.z += Mathf.Clamp(-4, -18.12f, 0.1f);
            camFollowPlayer.FollowOffset.z -= 0.08f * speedFarCam;

            countFresnel += 0.02f;

            fres = Mathf.Clamp(countFresnel, 0, 4);

            countFresnel = fres;

            segundoMaterial.SetFloat("_Fresnel_Power", countFresnel);

            if (camFollowPlayer.FollowOffset.z < -60)
            {
                GameManager.instance.Death(_player.gameObject, deathDuration, deathAudio, deathEffects);
                ShootPlayer();
                puede = false;
            }


            //yield return new WaitForSeconds(2f);
            yield return null;

        }
    }
        
    

    IEnumerator OutCamDeath()
    {
        isFlyDeath = true;

        yield return new WaitForSeconds(deathDuration);

        isFlyDeath = false;
    }

}
