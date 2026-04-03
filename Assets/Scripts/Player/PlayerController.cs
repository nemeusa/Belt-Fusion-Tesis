using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    FSM<TypeFSM> _fsm;


    [HideInInspector] public CharacterController _controller;
    [HideInInspector] public Vector2 _moveInput;
    [HideInInspector] public Vector3 _playerVelocity;

    [Header("Move")]
    [SerializeField] float _speed = 6f;
    [SerializeField] float _jumpHeight = 3f;
    public float _jumpFire = 3f;
    public float _gravityValue = -9.8f;
    [HideInInspector] public int jumpCount = 0;
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

    public event Action OnDashPressed;
    public event Action OnJumpPressed;

    private void Awake()
    {
        maxJumps = 1;
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



    public IEnumerator ActivateTrail(TrailRenderer trail)
    {
        trail.emitting = true;
        yield return new WaitForSeconds(0.4f);
        trail.emitting = false;

    }


    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            OnJumpPressed?.Invoke();

            if (_controller.isGrounded)
            {
                _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
                jumpCount++;
            }
        }
    }

    public void OnMove(InputValue value) { _moveInput = value.Get<Vector2>(); }
    public void OnDash(InputValue value) { if (value.isPressed) OnDashPressed?.Invoke(); }

    public void OnElement0(InputValue value){ if (value.isPressed) _fsm.ChangeState(TypeFSM.Default); }
    public void OnElement1(InputValue value){ if(value.isPressed) _fsm.ChangeState(TypeFSM.Fire); }
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