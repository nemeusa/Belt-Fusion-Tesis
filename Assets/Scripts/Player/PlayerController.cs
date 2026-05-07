using System;
using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public Vector2 moveInput;
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
    public AudioSource audioSource;
    private PaniniProjection panini;


    public List<ParticleSystem> changeElementVFX = new List<ParticleSystem>();

    [HideInInspector] public Material meshColors;

    [Header("Dash")]
    public bool isDashing;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    public bool invisibleInDash;
    public GameObject dashRingPar;
    public VisualEffect fbxDash;
    public VisualEffect fbxDash2;
    [HideInInspector] public int dashCount = 0;

    public event Action OnDashPressed;
    public event Action OnJumpPressed;

    private Coroutine transitionCoroutine;

    public Vector2 lookInput;

    public bool winGame;

    private bool _isBeingPushed = false;
    private Vector3 _pushDirection;

    public bool canMove = true;

    public RobotFollow robot;

    public AudioClip fireJumpAudio;
    public AudioClip dashAudio;
    public AudioClip walkAudio;
    public AudioClip changeElementAudio;

    private void Awake()
    {
        //GameManager.instance.player = this;
        _controller = GetComponent<CharacterController>();
        meshColors = meshChildren.GetComponent<SkinnedMeshRenderer>().material;
        canMove = true;
        robot = GameManager.instance.robot;
        audioSource = gameObject.GetComponent<AudioSource>();

        _fsm = new FSM<TypeFSM>();
        _fsm.AddState(TypeFSM.Default, new DefaultState(_fsm, this));
        _fsm.AddState(TypeFSM.Fire, new FireState(_fsm, this));
        _fsm.AddState(TypeFSM.Electricity, new ElectricityState(_fsm, this));
        _fsm.AddState(TypeFSM.Ice, new IceState(_fsm, this));

        _fsm.ChangeState(TypeFSM.Default);


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
        if (!canMove) return;

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
            if (winGame) animator.SetBool("Win", winGame);
            else
            {
                animator.SetFloat("Speed", moveInput.magnitude);
                animator.SetBool("IsGrounded", coyoteCounter > 0);
            }
        }
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
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
        if (boost < 0) boost = 0;
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

    public void ChangeElement(TypeFSM element)
    {
        GameManager.instance.BoostContainer.ChangeSymbol(element);
        robot.DispararRayo(element);

        audioSource.PlayOneShot(changeElementAudio);

        foreach (var a in changeElementVFX)
        {
            a.Play();
        }
    }

    public void DefaultPlayer()
    {
        _fsm.ChangeState(TypeFSM.Default);
        AddBoost(-5);
        winGame = false;
    }


    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
      
        if (!_isBeingPushed)
        {
            StartCoroutine(KnockbackRoutine(direction, force, duration));
        }
    }

    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        //moveInput = Vector2.zero;
        _fsm.ChangeState(TypeFSM.Default);

        canMove = false;
        _isBeingPushed = true;
        float timer = 0;

        while (timer < duration)
        {
            _controller.Move(direction * force * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        _isBeingPushed = false;
        canMove = true;
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
        if (!canMove) return;

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
        //Debug.Log(lookInput);
    }

    public void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }
    public void OnDash(InputValue value) { if (value.isPressed && canMove) OnDashPressed?.Invoke(); }

    public void OnElement0(InputValue value) { if (value.isPressed && canMove) _fsm.ChangeState(TypeFSM.Default); }
    public void OnElement1(InputValue value) { if (value.isPressed && canMove) _fsm.ChangeState(TypeFSM.Fire); }
    public void OnElement2(InputValue value) { if (value.isPressed && canMove) _fsm.ChangeState(TypeFSM.Electricity); }
    public void OnElement3(InputValue value) { if (value.isPressed && canMove) _fsm.ChangeState(TypeFSM.Ice); }
    public void OnPause(InputValue value) { if (value.isPressed) GameManager.instance.PauseGame(); }
}

public enum TypeFSM
{
    Default,
    Fire,
    Electricity,
    Ice,
    Slime
}