using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PortalCarousel : MonoBehaviour
{
    [System.Serializable]
    public class WorldEntry
    {
        public Transform portal;   // hijo del pivot
        public string panelID;     // panel de niveles a abrir (vacío si está bloqueado)
        public bool locked;
    }

    

    public MainMenu mainMenu;
    public MenuTransitioner transitioner;
    public PortalPanel panel;      // el PortalPanel de este panel de mundos
    public Camera cam;             // si queda vacío usa Camera.main
    public Transform pivot;        // centro del círculo; es lo que rota
    public WorldEntry[] worlds;

    [Header("Ajustes")]
    public float radius = 6f;
    public float rotateDuration = 0.5f;
    public float selectedScale = 1.2f;
    public float unselectedScale = 0.8f;
    public Vector3 facingOffsetEuler = Vector3.zero;  // poné (0,180,0) si se ven de espaldas

    int _index;
    float _yaw;
    float _yawVisual;
    Vector3[] _baseScale;
    Tween _rot;
    InputAction _left, _right, _submit;
    bool _shaking;

    [SerializeField] int vibration;
    [SerializeField] float elasticity;

    [Header("Efecto de transición")]
    public Volume transitionVolume;
    [Range(0f, 1f)] public float effectPeak = 1f;
    Tween _fx;

    void Awake()
    {
        if (cam == null) cam = Camera.main;

        // El "frente" del anillo (-Z local del pivot) apunta hacia la cámara
        Vector3 toPivot = pivot.position - cam.transform.position;
        toPivot.y = 0f;
        pivot.rotation = Quaternion.LookRotation(toPivot);
        _yaw = pivot.localEulerAngles.y;
        _yawVisual = _yaw;

        _baseScale = new Vector3[worlds.Length];
        float step = 360f / worlds.Length;
        for (int i = 0; i < worlds.Length; i++)
        {
            Transform p = worlds[i].portal;
            _baseScale[i] = p.localScale;
            float a = i * step * Mathf.Deg2Rad;
            p.localPosition = new Vector3(Mathf.Sin(a) * radius, 0f, -Mathf.Cos(a) * radius);

            
            //
        }

        _left = new InputAction("CarouselLeft", InputActionType.Button);
        _left.AddBinding("<Keyboard>/a");
        _left.AddBinding("<Keyboard>/leftArrow");
        _left.AddBinding("<Gamepad>/dpad/left");
        _left.AddBinding("<Gamepad>/leftStick/left");

        _right = new InputAction("CarouselRight", InputActionType.Button);
        _right.AddBinding("<Keyboard>/d");
        _right.AddBinding("<Keyboard>/rightArrow");
        _right.AddBinding("<Gamepad>/dpad/right");
        _right.AddBinding("<Gamepad>/leftStick/right");

        _submit = new InputAction("CarouselSubmit", InputActionType.Button);
        _submit.AddBinding("<Keyboard>/enter");
        _submit.AddBinding("<Keyboard>/space");
        _submit.AddBinding("<Gamepad>/buttonSouth");
    }

    void OnEnable()
    {
        _left.performed += OnLeft;
        _right.performed += OnRight;
        _submit.performed += OnSubmit;
        _left.Enable(); _right.Enable(); _submit.Enable();

        ApplyScales(true);
        panel.portal = worlds[_index].portal;
    }

    void OnDisable()
    {
        _left.performed -= OnLeft;
        _right.performed -= OnRight;
        _submit.performed -= OnSubmit;
        _left.Disable(); _right.Disable(); _submit.Disable();
    }

    void LateUpdate()
    {
        // Billboard en Y: todos miran siempre a la cámara, aunque el anillo gire
        foreach (var w in worlds)
        {
            Vector3 dir = w.portal.position - cam.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.001f) continue;
            w.portal.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(facingOffsetEuler);
        }
    }

    void OnLeft(InputAction.CallbackContext _) { Step(-1); }
    void OnRight(InputAction.CallbackContext _) { Step(1); }

    bool Busy => mainMenu.inputLocked || (transitioner != null && transitioner.IsBusy);

    void Step(int dir)
    {
        if (Busy) return;

        _index = (_index + dir + worlds.Length) % worlds.Length;
        _yaw += dir * (360f / worlds.Length);

        _rot?.Kill();
        _rot = DOTween.To(() => _yawVisual, v =>
            {
                _yawVisual = v;
                pivot.localRotation = Quaternion.Euler(0f, v, 0f);
            }, _yaw, rotateDuration)
            .SetEase(Ease.OutCubic);

        panel.portal = worlds[_index].portal;   // el que se va a atravesar
        ApplyScales(false);
        PlayTransitionFX();
    }

    void ApplyScales(bool instant)
    {
        for (int i = 0; i < worlds.Length; i++)
        {
            Vector3 target = _baseScale[i] * (i == _index ? selectedScale : unselectedScale);
            worlds[i].portal.DOKill();
            if (instant) worlds[i].portal.localScale = target;
            else worlds[i].portal.DOScale(target, rotateDuration).SetEase(Ease.OutCubic);
        }
    }

    void OnSubmit(InputAction.CallbackContext _)
    {
        if (Busy) return;

        WorldEntry w = worlds[_index];
        if (w.locked || string.IsNullOrEmpty(w.panelID))
        {
            ShakeLocked(w.portal);
            return;
        }
        mainMenu.OpenPanelByID(w.panelID);
    }
    void ShakeLocked(Transform portal)
    {
        _shaking = true;

        // "Derecha" de la cámara, aplanada, en espacio mundo
        Vector3 right = cam.transform.right;
        right.y = 0f;
        right.Normalize();

        // DOPunchPosition trabaja en espacio local: convertimos al del padre (el pivot)
        Vector3 localRight = portal.parent.InverseTransformDirection(right);

        portal.DOPunchPosition(localRight * 0.6f, 0.4f, vibrato: vibration, elasticity: elasticity)
              .OnComplete(() => _shaking = false)
              .OnKill(() => _shaking = false);
    }

    // Para ver el círculo en el editor sin darle play
    void OnDrawGizmosSelected()
    {
        if (pivot == null) return;
        Gizmos.color = Color.cyan;
        const int seg = 48;
        Vector3 prev = pivot.position + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= seg; i++)
        {
            float a = i * Mathf.PI * 2f / seg;
            Vector3 next = pivot.position + new Vector3(Mathf.Cos(a) * radius, 0f, Mathf.Sin(a) * radius);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
        Camera c = cam != null ? cam : Camera.main;
        if (c != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pivot.position, c.transform.position);
        }
    }

    void PlayTransitionFX()
    {
        if (transitionVolume == null) return;

        _fx?.Kill();
        transitionVolume.weight = 0f;

        _fx = DOTween.Sequence()
            .Append(DOTween.To(() => transitionVolume.weight, w => transitionVolume.weight = w,
                               effectPeak, rotateDuration * 0.35f).SetEase(Ease.OutQuad))
            .Append(DOTween.To(() => transitionVolume.weight, w => transitionVolume.weight = w,
                               0f, rotateDuration * 0.65f).SetEase(Ease.InQuad));
    }
}