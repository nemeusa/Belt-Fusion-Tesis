using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

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

    private CharacterController _controller;
    public PlayerController playerCode;
    public bool _isWallRunning = false;
    private RaycastHit _wallHit;

    Vector3 wallNormal;

    Collider[] hitColliders;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        playerCode = GetComponent<PlayerController>();
    }

    void Start()
    {
        playerCode.OnJumpPressed += JumpInWall;
        ogGravity = playerCode._gravityValue;
        ogVelocity = playerCode._playerVelocity;
    }

    void Update()
    {
        if(!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity)) return;

        if (_isWallRunning)
        {
            playerCode.CountMoves(0);
            playerCode.coyoteCounter = playerCode.coyoteTime * 2;
            playerCode.jumpCount = 0;
            playerCode.dashCount = 0;
            CheckForWall();
            playerCode.dontMove = true;
            playerCode._gravityValue = 0;
            DoWallRun();
        }
        else
        {
            if (playerCode._gravityValue != ogGravity || playerCode.dontMove)
            {
                playerCode.dontMove = false;
                playerCode._gravityValue = ogGravity;

            }
        }
    }

    void CheckForWall()
    {
        if (_controller.isGrounded || isJumping)
        {
            _isWallRunning = false;

            return;
        }
        Vector3 origin = transform.position;


        hitColliders = Physics.OverlapSphere(transform.position, sphereRadius, wallLayer);

        if (hitColliders.Length > 0)
        {
            Vector3 closestPoint = hitColliders[0].ClosestPoint(origin);
            Vector3 directionToWall = (closestPoint - origin).normalized;

    
            if (Physics.Raycast(origin, directionToWall, out _wallHit, sphereRadius + 0.5f, wallLayer))
            {

                wallNormal = _wallHit.normal;

                Physics.Raycast(transform.position, (hitColliders[0].transform.position - transform.position).normalized, out _wallHit, sphereRadius + 0.5f, wallLayer);
                _isWallRunning = true;
            }
        }
        else
        {
            _isWallRunning = false;
        }


    }

    void DoWallRun()
    {
        WallData data = null;
        if (hitColliders.Length > 0)
            data = hitColliders[0].GetComponent<WallData>();


        Vector3 wallForward;

        if (data != null) wallForward = data.transform.TransformDirection(data.runDirection);

        else  wallForward = Vector3.ProjectOnPlane(transform.forward, _wallHit.normal).normalized;
        

        Vector3 move = wallForward * wallRunSpeed;

        _controller.Move(move * Time.deltaTime);

        transform.forward = move;


    }

    void JumpInWall()
    {
        if (!playerCode._fsm.WhatCurrentState(TypeFSM.Electricity)) return;

        if (_isWallRunning) StartCoroutine(WaitJump());
            else CheckForWall();
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

    private void OnDrawGizmos()
    {
        //Gizmos.color = _isWallRunning ? Color.green : Color.red;

        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
