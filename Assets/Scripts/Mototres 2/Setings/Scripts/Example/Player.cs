using GameOptions;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// EJEMPLO de como un script de gameplay consume la configuracion.
/// Camara en primera persona que usa la sensibilidad elegida en el menu.
///
/// Patron: suscribirse a SettingsApplied y guardar el valor en un campo.
/// Asi el Player toma el valor nuevo apenas se aplica, sin consultar al manager en cada frame,
/// y no necesita saber nada del menu ni de como se guarda la configuracion.
/// </summary>
public sealed class Player : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAction; // Ej: Player/Look del InputSystem_Actions
    [SerializeField] private Transform cameraPivot;           // La camara (hija del Player)

    [Tooltip("Grados de giro por píxel de movimiento del mouse con sensibilidad 1.")]
    [SerializeField] private float degreesPerPixel = 0.1f;
    [SerializeField] private float maxPitch = 85f;

    private float _sensitivity = 1f;
    private float _pitch;

    private void OnEnable()
    {
        lookAction.action.Enable();

        if (SettingsManager.Instance == null)
            return; // Sin manager se usa la sensibilidad por defecto

        SettingsManager.Instance.SettingsApplied += OnSettingsApplied;
        OnSettingsApplied(SettingsManager.Instance.Current); // Valor inicial
    }

    private void OnDisable()
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SettingsApplied -= OnSettingsApplied;
    }

    private void OnSettingsApplied(GameSettings settings) => _sensitivity = settings.mouseSensitivity;

    private void Update()
    {
        // Con el cursor libre (menu abierto) la camara no se mueve.
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        // El delta del mouse ya es "por frame": no se multiplica por Time.deltaTime.
        Vector2 look = lookAction.action.ReadValue<Vector2>() * (degreesPerPixel * _sensitivity);

        transform.Rotate(0f, look.x, 0f);                              // Giro horizontal: todo el cuerpo
        _pitch = Mathf.Clamp(_pitch - look.y, -maxPitch, maxPitch);    // Giro vertical: solo la cámara
        cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}
