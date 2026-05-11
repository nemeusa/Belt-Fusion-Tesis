using System.Collections;
using UnityEngine;

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
    public bool _isWallRunning = false;
    private RaycastHit _wallHit;

    Vector3 wallNormal;

    Collider[] hitColliders;

    Vector3 _direccionDash;

    WallData data;

    bool inUse;

    private void Awake()
    {
        //_controller = GetComponent<CharacterController>();
        //playerCode = GetComponent<PlayerController>();
    }

    void Start()
    {
        playerCode.OnJumpPressed += JumpInWall;
        playerCode.OnDashPressed += JumpInWall;
        ogGravity = playerCode._gravityValue;
        ogVelocity = playerCode._playerVelocity;
    }

    void Update()
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity)) if (_isWallRunning) _isWallRunning = false;

        if (_isWallRunning) DoWallRun();

        else
        {
            if (data != null || playerCode.dontMove || playerCode._gravityValue != ogGravity)
            {
                data = null;
                playerCode.dontMove = false;
                playerCode._gravityValue = ogGravity;

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity)) return;

        if (other.TryGetComponent<WallData>(out WallData wall))
        {
            data = wall;
            _isWallRunning = true;
            StartCoroutine(DashEffects());

            playerCode.CountMoves(0);
            playerCode.dontMove = true;
            playerCode._gravityValue = 0;


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity) || inUse) return;

        if (other.TryGetComponent<WallData>(out WallData wall))
        {
            Debug.Log("usada");
            data = wall;
            _isWallRunning = true;
            StartCoroutine(DashEffects());

            playerCode.CountMoves(0);
            playerCode.dontMove = true;
            playerCode._gravityValue = 0;

            inUse = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<WallData>(out WallData wall))
        {
            _isWallRunning = false;
            data = null;

            playerCode.coyoteCounter = playerCode.coyoteTime * 2;
            playerCode.jumpCount = 0;
            playerCode.dashCount = 0;
            playerCode.dontMove = false;
            playerCode._gravityValue = ogGravity;

            inUse = false;
        }
    }

    void DoWallRun()
    {

        Vector3 wallForward;

        if (data != null) wallForward = data.transform.TransformDirection(data.runDirection);

        else wallForward = Vector3.ProjectOnPlane(transform.forward, _wallHit.normal).normalized;


        Vector3 move = wallForward * wallRunSpeed;

        playerCode._controller.Move(move * Time.deltaTime);

        transform.forward = move;


        playerCode.coyoteCounter = playerCode.coyoteTime * 2;
        playerCode.jumpCount = 0;
        playerCode.dashCount = 0;

    }

    void JumpInWall()
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity)) return;

        if (_isWallRunning) StartCoroutine(WaitJump());
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
        while (_isWallRunning)
        {
            createEffects++;
            playerCode.audioSource.PlayOneShot(playerCode.dashAudio);

            playerCode.fbxDash.SendEvent("OnPlay");
            IniciarDash();
            playerCode.fbxDash2.SendEvent("OnPlay");

            Debug.Log("efecto" + createEffects);

            yield return new WaitForSeconds(0.05f);

            playerCode.fbxDash.SendEvent("OnStop");
            playerCode.fbxDash2.SendEvent("OnStop");

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

        var d = GameObject.Instantiate(playerCode.dashRingPar, playerCode.ElectricityTrail.transform.position, Quaternion.identity);
        d.transform.forward = _direccionDash;
        GameObject.Destroy(d, 2);
    }

    //private void OnDrawGizmos()
    //{
    //    //Gizmos.color = _isWallRunning ? Color.green : Color.red;

    //    Gizmos.DrawWireSphere(transform.position, sphereRadius);
    //}
}
