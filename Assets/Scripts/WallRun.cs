using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WallRun : MonoBehaviour
{
    [Header("Detección")]
    public LayerMask wallLayer;
    public float sphereRadius = 0.5f;     // Radio de la esfera de detección
    public float detectionDistance = 1f;  // Qué tanto busca hacia adelante/lados

    [Header("Movimiento")]
    public float wallRunSpeed = 10f;
    public float jumpForce = 8f;
    float ogGravity; // Gravedad reducida para que caiga lento
    Vector3 ogVelocity;
    bool isJumping;

    public PlayerController playerCode;
    private RaycastHit _wallHit;

    Vector3 wallNormal;

    Collider[] hitColliders;

    Vector3 _direccionDash;

    WallData data;

    bool inUse;

    Vignette vignette;

    bool canDetect = true;


    void Start()
    {
        playerCode.OnJumpPressed += JumpInWall;
        playerCode.OnDashPressed += JumpInWall;
        ogGravity = playerCode._gravityValue;
        ogVelocity = playerCode._playerVelocity;

    }

    void Update()
    {
        if (playerCode.isDeath) return;

        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity))
            if (playerCode.isWallRunning)
            {
                data = null;
                playerCode.dontMovePlayer = false;
                playerCode._gravityValue = ogGravity;
                inUse = false;
                playerCode.isWallRunning = false;
            }


        if (playerCode.isWallRunning) DoWallRun();

        //else
        //{
        //    if (data == null || playerCode.dontMovePlayer || playerCode._gravityValue != ogGravity)
        //    {
        //        data = null;
        //        playerCode.dontMovePlayer = false;
        //        playerCode._gravityValue = ogGravity;

        //    }

        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity) || !canDetect || playerCode.isDeath) return;

        if (other.TryGetComponent<WallData>(out WallData wall))
        {
            data = wall;

            EnterWallCode();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity) || inUse || !canDetect || playerCode.isDeath) return;

        if (other.TryGetComponent<WallData>(out WallData wall))
        {
            data = wall;
            EnterWallCode();

           
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<WallData>(out WallData wall))
        {
            playerCode.isWallRunning = false;
            data = null;

            if (playerCode._fsm.WhatCurrentState(TypeFSM.Electricity))
            {

                playerCode.coyoteCounter = playerCode.coyoteTime * 2;
                playerCode.jumpCount = 0;
                playerCode.dashCount = 0;
            }
            playerCode.dontMovePlayer = false;
            playerCode._gravityValue = ogGravity;

            inUse = false;
        }
    }

    void EnterWallCode()
    {
        StartCoroutine(CanDet());
        playerCode.isWallRunning = true;
        StartCoroutine(DashEffects());

        playerCode.CountMoves(0);
        playerCode.dontMovePlayer = true;
        playerCode._gravityValue = 0;

        inUse = true;
    }

    IEnumerator CanDet()
    {
        canDetect = false;

        yield return new WaitForSeconds(0.1f);

        canDetect = true;
    }

    void DoWallRun()
    {

        Vector3 wallForward;

        if (data != null) wallForward = data.transform.TransformDirection(data.runDirection);

        else wallForward = Vector3.ProjectOnPlane(transform.forward, _wallHit.normal).normalized;


        Vector3 move = wallForward * wallRunSpeed;

        //playerCode._controller.Move(move * Time.unscaledDeltaTime);
        playerCode._controller.Move(move * Time.deltaTime);

        transform.forward = move;


        playerCode.coyoteCounter = playerCode.coyoteTime * 2;
        playerCode.jumpCount = 0;
        playerCode.dashCount = 0;

    }

    void JumpInWall()
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity)) return;

        if (playerCode.isWallRunning) StartCoroutine(WaitJump());
        //else
        //if (!playerCode._controller.isGrounded) CheckForWall();
    }

    IEnumerator WaitJump()
    {
        playerCode._playerVelocity = (Vector3.up * jumpForce) + (wallNormal * jumpForce);
        isJumping = true;
        playerCode.jumpCount++;

        yield return new WaitForSeconds(0.3f);

        playerCode._playerVelocity = ogVelocity;
        isJumping = false;
    }

    IEnumerator DashEffects()
    {
        int createEffects = 0;
        while (playerCode.isWallRunning)
        {
            createEffects++;
            playerCode.audioSource.PlayOneShot(playerCode.dashAudio);

            foreach (var d in playerCode.fbxDashList) d.SendEvent("OnPlay");
            IniciarDash();

            //Debug.Log("efecto" + createEffects);

            yield return new WaitForSeconds(0.05f);

            foreach (var d in playerCode.fbxDashList) d.SendEvent("OnStop");


            yield return null;

        }

    }

    void IniciarDash()
    {
        Vector3 inputDireccion = new Vector3(playerCode.moveInput.x, 0, playerCode.moveInput.y);

        if (inputDireccion.sqrMagnitude > 0.1f)
        {
            _direccionDash = inputDireccion.normalized;
        }
        else
        {
            _direccionDash = playerCode.transform.forward;
        }

        foreach (var p in playerCode.dashParticules)
        {

            var d = GameObject.Instantiate(p, playerCode.transform.position, Quaternion.identity);
            d.transform.forward = _direccionDash;
            GameObject.Destroy(d, 2);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    //Gizmos.color = _isWallRunning ? Color.green : Color.red;

    //    Gizmos.DrawWireSphere(transform.position, sphereRadius);
    //}
}
