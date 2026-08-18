using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region vars
    [HideInInspector] public FSM<TypeFSM> _fsm;

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
    public int maxJumps = 1;
    public float agarreDelPiso = 15f; // 15 es fricción normal (frena rápido)
    private Vector3 _currentMoveVelocity;
    float ogGravity;
    [SerializeField] AudioClip _audioBoost;


    [Header("Skills")]
    [SerializeField] int maxBoost = 3;
    public int boost { get; private set; }


    [Header("References")]
    [SerializeField] GameObject meshChildren;
    [SerializeField] GameObject[] meshEyes;
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
    public CinemachineCamera jumpCamTarget;
    public CinemachineCamera meteoriteCamTarget;
    public Material boostMat;
    public GameObject meshFather;
    public Vector3 meshFatherDefaultPos;
    public Material fMat, electricityMat, fireMat, iceMat;
    public Material fMatEye, electricityMatEye, fireMatEye, iceMatEye;

    private PaniniProjection panini;


    public List<ParticleSystem> changeElementVFX = new List<ParticleSystem>();

    [HideInInspector] public Material meshColors;

    [Header("Dash")]
    public bool isDashing;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    public GameObject dashRingPar;
    public VisualEffect fbxDash;
    public VisualEffect fbxDash2;
    [HideInInspector] public int countMovs;


    [HideInInspector] public int dashCount = 0;

    public event Action OnDashPressed;
    public event Action OnJumpPressed;

    private Coroutine transitionCoroutine;

    public Vector2 lookInput;


    private Vector3 _pushDirection;

    [Header("Texts")]
    public TMP_Text countMovsText;
    public TMP_Text countCrystalsText;
    public GameObject xButtomRepeat;
    public GameObject trianguleButtom;



    public GameObject respawnEffectsPrefabs;
    public Transform respawnEffectsPoint;

    [Header("Bool")]
    public bool isDeath = false;
    public bool dontMovePlayer;
    public bool dontDobleJump;
    public bool dontChangeElement;
    public bool is2Dmoving = false;
    public bool isIntoMeteorite;
    private bool _isBeingPushed = false;
    public bool winGame;
    [HideInInspector] public bool isWallRunning = false;
    public bool invisibleInDash;


    public RobotFollow robot;

    int mount;

   [SerializeField] Animator boostCanvasAni;

    [Header("Audio")]
    public AudioClip fireJumpAudio;
    public AudioClip dashAudio;
    public AudioClip walkAudio;
    public AudioClip changeElementAudio;

    #endregion

    private void Awake()
    {
        //GameManager.instance.player = this;

        _controller = GetComponent<CharacterController>();
        meshColors = meshChildren.GetComponent<SkinnedMeshRenderer>().material;
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
        meshFatherDefaultPos = meshFather.transform.localPosition;
        StartCoroutine(StartGame());
        CountMoves(0);
        GameManager.instance.BoostContainer.BoostsActive(boost);
        coyoteTime = 0.2f;
        maxJumps = 1;
        initialSpeed = speed;
        ogGravity = _gravityValue;
        if (globalVolume.profile.TryGet<PaniniProjection>(out var tmpPanini))
        {
            //Debug.Log("Encontro el panini");
            panini = tmpPanini;
        }


    }

    void Update()
    {
        if (isDeath) return;

        _fsm.Execute();

        if (dontMovePlayer) return;
        JumpLogics(_controller.isGrounded, _playerVelocity.y < 0);
        MovePlayer();

        if (animator != null)
        {
            if (winGame) animator.SetBool("Win", winGame);
            else
            {
                if (!dontMovePlayer) animator.SetFloat("Speed", moveInput.magnitude);
                animator.SetBool("IsGrounded", coyoteCounter > 0);
            }
        }
    }

    #region Movement
    void MovePlayer()
    {

        if (dontMovePlayer) return;

        Vector3 move;

        if (is2Dmoving) move = new Vector3(moveInput.x, 0, moveInput.y * 0.8f);
        

        else 
            move = new Vector3(moveInput.x, 0, moveInput.y);

        _currentMoveVelocity = Vector3.Lerp(_currentMoveVelocity, move * speed, Time.deltaTime * agarreDelPiso);

        // Y le pasamos esa velocidad suavizada al CharacterController en vez del 'move' directo
        _controller.Move(_currentMoveVelocity * Time.deltaTime);
        //_controller.Move(move * Time.deltaTime * speed);


        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }

    public void JumpLogics(bool condition, bool conditionTwo)
    {

        if (condition)
        {
            if (conditionTwo)
            {
                _playerVelocity.y = -2f;
                jumpCount = 0;
                dashCount = 0;
            }
                jumpCamTarget.Priority = 1;

            if (!isDashing) CountMoves(0);
            coyoteCounter = coyoteTime;
        }
        else if (!is2Dmoving && !isIntoMeteorite)jumpCamTarget.Priority = 20;

        if (!isDashing) coyoteCounter -= Time.deltaTime;

        if (coyoteCounter > 0f) dashCount = 0;
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

        isDeath = true;
        _isBeingPushed = true;
        float timer = 0;

        while (timer < duration)
        {
            _controller.Move(direction * force * Time.deltaTime);
            //_controller.Move(direction * force * Time.unscaledDeltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        _isBeingPushed = false;
        isDeath = false;
    }

    IEnumerator StartGame()
    {
        isDeath = true;
        yield return new WaitForSeconds(0.4f);
        isDeath = false;
        GameManager.instance.BoostContainer.seconds = 0;
    }

    #endregion

    #region Extra

    public void AddBoost(int newBoost)
    {
        if (boost + newBoost > maxBoost)
        {
            Debug.Log($"Se supero el boost. boost: {boost}");
            return;
        }


        boost += newBoost;
        if (boost <= 0) boost = 0;
        else StartCoroutine(BoostAni());

        GameManager.instance.BoostContainer.BoostsActive(boost);

        if(newBoost > 0) GameManager.instance.PlaySound(_audioBoost);

    }

    IEnumerator BoostAni()
    {
        boostCanvasAni.SetBool("Intro", true);
        yield return new WaitForSeconds(0.5f);
        boostCanvasAni.SetBool("Intro", false);
    }

    public void ChangeMatElement(Material newMatBody, Material newMatEye)
    {
        meshChildren.GetComponent<SkinnedMeshRenderer>().material = newMatBody;

        foreach (var m in meshEyes)
        m.GetComponent<SkinnedMeshRenderer>().material = newMatEye;
    }


    public void CountCrystals(int cant)
    {
        mount += cant;
        countCrystalsText.text = "A " + mount;

        if (mount >= 10)
        {
            trianguleButtom.SetActive(true);
        }
        else
        {
            trianguleButtom.SetActive(false);
        }

    }

    public void CountMoves(int n)
    {
        if (n <= 0) countMovs = 0;
        else  countMovs += n;

        if (countMovs < 2) countMovsText.text = "";
        else countMovsText.text = "combo: " + countMovs;

    }

    public void ChangeElement(TypeFSM element)
    {
        GameManager.instance.BoostContainer.ChangeSymbol(element);
        //robot.DispararRayo(element);

        if (isDeath) return; 

        audioSource.PlayOneShot(changeElementAudio);

        VibrarControl(0.2f, 0.4f, 0.2f);


        foreach (var a in changeElementVFX)
        {
            a.Play();
        }
    }

    public void DeathPlayer()
    {
        isDeath = true;
        isWallRunning = false;
        isDashing = false;
        isIntoMeteorite = false;
        _controller.enabled = false;
        agarreDelPiso = 15;

        //_controller.Move(Vector3.zero);
    }

    public void RespawnPlayer()
    {
        _controller.enabled = true;
        isDeath = false;
        DefaultPlayer();
        var c = Instantiate(respawnEffectsPrefabs, respawnEffectsPoint.position, Quaternion.identity);
        Destroy(c, 2);
        //meshFather.transform.position = meshFatherDefaultPos;
        meshFather.transform.localPosition = Vector3.zero;
    }

    public void DefaultPlayer()
    {
        _fsm.ChangeState(TypeFSM.Default);
        AddBoost(-5);
        _gravityValue = ogGravity;
        winGame = false;
    }

    #endregion

    #region Inputs
    public void OnReload(InputValue value)
    {
        if (GameManager.instance._pauseCode.juegoPausado) return;

        if (value.isPressed)
        {
            SceneManager.LoadScene(NextLevel._nextLevel);
            Debug.Log("Se cargo xd");
        }

    }

    public void OnJump(InputValue value)
    {
        if (isDeath || GameManager.instance._pauseCode.juegoPausado) return;

        if (value.isPressed)
        {
            OnJumpPressed?.Invoke();

            if (coyoteCounter > 0f && jumpCount == 0)
            {
                coyoteCounter = 0f;
                jumpCount++;
                _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);

            }

            //Debug.Log("toco origen");
        }
    }


    public void OnDebbie(InputValue value)
    {
        if (value.isPressed && !isDeath && mount >= 10 && !GameManager.instance._pauseCode.juegoPausado)
        {
            //robot.meshRobot.material.color = Color.blue;
            //foreach (var m in robot.meshRobot) m.material.color = UnityEngine.Random.ColorHSV();

            //TimeManager.Instance.ActivarCamaraLenta();

            StartCoroutine(TimeSlow.Instance.BulletTimeRoutine());

            CountCrystals(-10);
        }
    }


    public void OnBoost(InputValue value) { if (!GameManager.instance._pauseCode.juegoPausado) AddBoost(1); }


    public void OnLook(InputValue value)
    {
        if (GameManager.instance._pauseCode.juegoPausado) return;
            lookInput = value.Get<Vector2>();
        //Debug.Log(lookInput);
    }

    public void OnMove(InputValue value) { if (!GameManager.instance._pauseCode.juegoPausado) moveInput = value.Get<Vector2>(); }
    public void OnDash(InputValue value) { if (value.isPressed && !isDeath && !GameManager.instance._pauseCode.juegoPausado) OnDashPressed?.Invoke(); }

    public void OnElement0(InputValue value) { if (value.isPressed && !isDeath && !dontChangeElement && !GameManager.instance._pauseCode.juegoPausado) _fsm.ChangeState(TypeFSM.Default); }
    public void OnElement1(InputValue value) { if (value.isPressed && !isDeath && !dontChangeElement && !GameManager.instance._pauseCode.juegoPausado) _fsm.ChangeState(TypeFSM.Fire); }
    public void OnElement2(InputValue value) { if (value.isPressed && !isDeath && !dontChangeElement && !GameManager.instance._pauseCode.juegoPausado) _fsm.ChangeState(TypeFSM.Electricity); }
    public void OnElement3(InputValue value) { if (value.isPressed && !isDeath && !dontChangeElement && !GameManager.instance._pauseCode.juegoPausado) _fsm.ChangeState(TypeFSM.Ice); }
    public void OnPause(InputValue value) { if (value.isPressed) GameManager.instance.PauseGame(); }


    public void VibrarControl(float intensidadBaja, float intensidadAlta, float duracion)
    {
        // Conseguimos el joystick actual que está usando el jugador
        Gamepad mandoActual = Gamepad.current;

        // Validamos que haya un joystick conectado para que no tire error en PC con teclado
        if (mandoActual != null)
        {
            // Activamos los motores con las intensidades (van de 0.0f a 1.0f)
            mandoActual.SetMotorSpeeds(intensidadBaja, intensidadAlta);

            // Cancelamos la vibración automáticamente después del tiempo que le digas
            Invoke(nameof(ApagarVibracion), duracion);
        }
    }

    private void ApagarVibracion()
    {
        Gamepad mandoActual = Gamepad.current;
        if (mandoActual != null)
        {
            mandoActual.ResetHaptics(); // Apaga todos los motores de golpe
        }
    }

    // Al cerrar el juego o cambiar de escena, nos aseguramos de apagar el motor 
    // para que el joystick no se quede vibrando infinitamente arriba de la mesa
    private void OnDisable()
    {
        Gamepad mandoActual = Gamepad.current;
        if (mandoActual != null) mandoActual.ResetHaptics();
    }

    #endregion

    #region Visual effects
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

    #endregion
}

public enum TypeFSM
{
    Default,
    Fire,
    Electricity,
    Ice,
    Slime
}