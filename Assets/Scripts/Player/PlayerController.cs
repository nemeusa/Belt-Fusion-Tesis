using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    FSM<TypeFSM> _fsm;


    [HideInInspector] public CharacterController _controller;
    [HideInInspector] public Vector2 _moveInput;
    [HideInInspector] public Vector3 _playerVelocity;

    [Header("Move")]
    public float speed = 6f;
    [SerializeField] float _jumpHeight = 3f;
    public float _jumpFire = 3f;
    public float _gravityValue = -9.8f;
    [HideInInspector] public float coyoteTime = 0.2f;
    [HideInInspector] public float coyoteCounter;
    [HideInInspector] public float initialSpeed;
    public int jumpCount = 0;
    [HideInInspector] public int maxJumps = 1;

    [Header("Skills")]
    [SerializeField] int maxBoost = 3;
    public int boost { get; private set; }


    [Header("References")]
    [SerializeField] GameObject meshChildren;
    public Animator animator;
    public TrailRenderer fireTrail;
    public ParticleSystem fireParticleTrail;
    public TrailRenderer ElectricityTrail;
    public ParticleSystem electricityParticleTrail;
    public GameObject fireBall;
    public Transform firePoint;
    public GameObject explosionJumpPrefab;
    public GameObject fireAura;
    public GameObject energyAura;
    public Volume globalVolume;
    private PaniniProjection panini;

    [HideInInspector] public Material meshColors;

    [Header("Dash")]
    public bool isDashing;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    [HideInInspector] public int dashCount = 0;

    public event Action OnDashPressed;
    public event Action OnJumpPressed;

    private Coroutine transitionCoroutine;

    public Vector2 lookInput;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        meshColors = meshChildren.GetComponent<SkinnedMeshRenderer>().material;

        _fsm = new FSM<TypeFSM>();
        _fsm.AddState(TypeFSM.Default, new DefaultState(_fsm, this));
        _fsm.AddState(TypeFSM.Fire, new FireState(_fsm, this));
        _fsm.AddState(TypeFSM.Electricity, new ElectricityState(_fsm, this));
        _fsm.AddState(TypeFSM.Ice, new IceState(_fsm, this));

        _fsm.ChangeState(TypeFSM.Default);

        GameManager.instance.player = this;
    }

    private void Start()
    {
        GameManager.instance.BoostContainer.BoostsActive(boost);
        coyoteTime = 0.2f;
        maxJumps = 1;
        initialSpeed = speed;
        if (globalVolume.profile.TryGet<PaniniProjection>(out var tmpPanini))
        {
            Debug.Log("Encontro el panini");
            panini = tmpPanini;
        }

    }

    void Update()
    {
        _fsm.Execute();

        if (_controller.isGrounded)
        {
            if (_playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
                jumpCount = 0;
                dashCount = 0;

            }
            coyoteCounter = coyoteTime;
        }
        else coyoteCounter -= Time.deltaTime;

        MovePlayer();

        if (animator != null)
        {
            animator.SetFloat("Speed", _moveInput.magnitude);
            animator.SetBool("IsGrounded", coyoteCounter > 0);
        }
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        _controller.Move(move * Time.deltaTime * speed);

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }

    public void AddBoost(int newBoost)
    {
        if (boost + newBoost > maxBoost)
        {
            Debug.Log($"Se supero el boost. boost: {boost}");
            return;
        }

        boost += newBoost;
        //GameManager.instance.boostText.text = $"Boost: {boost}";
        GameManager.instance.BoostContainer.BoostsActive(boost);
    }

    public IEnumerator ActivateTrail(TrailRenderer trail)
    {
        trail.emitting = true;
        yield return new WaitForSeconds(0.4f);
        trail.emitting = false;

    }

    public void SetPaniniIntensity(float value)
    {
        if (panini != null)
        {
            panini.distance.overrideState = true;
            panini.distance.value = value;
            Debug.Log("cambia lol");
        }
    }

    public IEnumerator ActivateParticleTrail(ParticleSystem trail)
    {
        ChangePaniniSmooth(0.3f, 0.5f);

        trail.Play();
        yield return new WaitForSeconds(0.4f);
        trail.Stop();
        ChangePaniniSmooth(0, 0.5f);

    }


    public void ChangePaniniSmooth(float targetIntensity, float duration)
    {
        if (panini == null) return;

        // Si ya hay una transición en curso, la detenemos para empezar la nueva
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(SmoothTransition(targetIntensity, duration));
    }

    private IEnumerator SmoothTransition(float target, float duration)
    {
        float time = 0;
        float startValue = panini.distance.value;
        panini.distance.overrideState = true;

        while (time < duration)
        {
            // Interpolación lineal suave
            panini.distance.value = Mathf.Lerp(startValue, target, time / duration);
            time += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        panini.distance.value = target;
    }

    public void OnReload(InputValue value)
    {
        if (value.isPressed)
        {
            SceneManager.LoadScene(NextLevel._nextLevel);
            Debug.Log("Se cargo xd");
        }

    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {

            OnJumpPressed?.Invoke();

            if (coyoteCounter > 0f && jumpCount == 0)
            {
                coyoteCounter = 0f;
                jumpCount++;
                _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);

            }

        }
    }

    public void OnBoost(InputValue value) { AddBoost(1); }


    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        Debug.Log(lookInput);
    }

    public void OnMove(InputValue value) { _moveInput = value.Get<Vector2>(); }
    public void OnDash(InputValue value) { if (value.isPressed) OnDashPressed?.Invoke(); }

    public void OnElement0(InputValue value) { if (value.isPressed) _fsm.ChangeState(TypeFSM.Default); }
    public void OnElement1(InputValue value) { if (value.isPressed) _fsm.ChangeState(TypeFSM.Fire); }
    public void OnElement2(InputValue value) { if (value.isPressed) _fsm.ChangeState(TypeFSM.Electricity); }
    public void OnElement3(InputValue value) { if (value.isPressed) _fsm.ChangeState(TypeFSM.Ice); }
}

public enum TypeFSM
{
    Default,
    Fire,
    Electricity,
    Ice,
    Slime
}