using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    private CinemachineCameraOffset _offsetExtension;
    private Vector2 _lookInput;

    [Header("Configuración")]
    public float maxOffset = 5f;    // Qué tanto puede subir/bajar
    public float sensitivity = 3f;  // Velocidad de respuesta
    public float returnSpeed = 2f;  // Velocidad de retorno al soltar
    public float waitBeforeReturn = 1.5f; // Segundos de espera

    public float _currentYOffset = 0f;
    public float _returnTimer = 0f;

    void Start()
    {
        _offsetExtension = GetComponent<CinemachineCameraOffset>();
    }

    // Vinculado al Input System (Stick Izquierdo)
    void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();

        Debug.Log(_lookInput);

        // Si hay movimiento, reseteamos el timer de espera
        if (_lookInput.y != 0)
        {
            _returnTimer = waitBeforeReturn;
        }
    }

    void Update()
    {
        //if (_returnTimer > 0)
        //{
        // Mientras el timer esté activo, aplicamos el input
        _currentYOffset += _lookInput.y * sensitivity * Time.deltaTime;
        _currentYOffset = Mathf.Clamp(_currentYOffset, -maxOffset, maxOffset);
        _returnTimer -= Time.deltaTime;
        //}
        //else
        //{
        //    // Cuando el timer llega a 0, vuelve suavemente al centro (0)
        //    _currentYOffset = Mathf.Lerp(_currentYOffset, 0, returnSpeed * Time.deltaTime);
        //}

        // Aplicamos el valor final a la extensión de Cinemachine
        _offsetExtension.Offset.y = _currentYOffset;
    }
}
