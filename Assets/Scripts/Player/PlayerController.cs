using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    FSM<TypeFSM> _fsm;


    public CharacterController _controller;
    Vector2 _moveInput;
    [HideInInspector] public Vector3 _playerVelocity;

    [Header("Move")]
    [SerializeField] float _speed = 6f;
    [SerializeField] float _jumpHeight = 3f;
    public float _jumpFire = 3f;
    public float _gravityValue = -9.8f;
    [SerializeField] int jumpCount = 0;
    [HideInInspector] public int maxJumps = 1;

    [Header("References")]
    public TrailRenderer fireTrail;
    public TrailRenderer ElectricityTrail;
    public GameObject fireBall;
    public Transform firePoint;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    [HideInInspector] public bool canDash = false;
    private bool isDashing;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        _fsm = new FSM<TypeFSM>();
        _fsm.AddState(TypeFSM.Default, new DefaultState(_fsm, this));
        _fsm.AddState(TypeFSM.Fire, new FireState(_fsm, this));
        _fsm.AddState(TypeFSM.Electricity, new ElectricityState(_fsm, this));
        _fsm.AddState(TypeFSM.Ice, new IceState(_fsm, this));

        _fsm.ChangeState(TypeFSM.Default);
    }

    void Update()
    {
        _fsm.Execute();

        if (_controller.isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
            jumpCount = 0;
        }

        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        _controller.Move(move * Time.deltaTime * _speed);

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }



    IEnumerator ActivateTrail(TrailRenderer trail)
    {
        trail.emitting = true;
        yield return new WaitForSeconds(0.4f);
        trail.emitting = false;

    }

    private IEnumerator ExecuteDash()
    {
        isDashing = true;

        float originalGravity = _playerVelocity.y;
        _playerVelocity.y = 0;

        Vector3 dashDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        if (dashDirection == Vector3.zero) dashDirection = transform.forward;

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            _controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
    }
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (_controller.isGrounded || jumpCount < maxJumps)
            {
                if (jumpCount < 1) _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
                else
                {
                    _playerVelocity.y = Mathf.Sqrt(_jumpFire * -3.0f * _gravityValue);
                    StartCoroutine(ActivateTrail(fireTrail));
                    Instantiate(fireBall, firePoint.transform.position, Quaternion.identity);
                }

                jumpCount++;
            }
        }
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash && !isDashing)
        {
            StartCoroutine(ExecuteDash());
            StartCoroutine(ActivateTrail(ElectricityTrail));

        }
    }

    public void OnElement0(InputValue value)
    {
        if (value.isPressed) _fsm.ChangeState(TypeFSM.Default);
    }

    public void OnElement1(InputValue value)
    {
        if(value.isPressed) _fsm.ChangeState(TypeFSM.Fire);
    }

    public void OnElement2(InputValue value){ if (value.isPressed) _fsm.ChangeState(TypeFSM.Electricity); }
    public void OnElement3(InputValue value){ if (value.isPressed) _fsm.ChangeState(TypeFSM.Ice); }
}

public enum TypeFSM
{
    Default,
    Fire,
    Electricity,
    Ice,
    Slime
}