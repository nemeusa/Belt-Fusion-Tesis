using Unity.Cinemachine;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private CinemachineCameraOffset _offsetExtension;
    CinemachineFollow _camFollow;
    PlayerController player;
    //Vector2 _lookInput;

    [Header("Configuración")]
    public float maxOffset = 5f;    // Qué tanto puede subir/bajar
    public float sensitivity = 3f;  // Velocidad de respuesta
    public float returnSpeed = 2f;  // Velocidad de retorno al soltar
    public float waitBeforeReturn = 1.5f; // Segundos de espera

    public float _currentYOffset = 0f;
    public float _returnTimer = 0f;

    Vector2 defaultAlture;

    void Start()
    {
        _camFollow = GetComponent<CinemachineFollow>();


        player = GameManager.instance.player;
        defaultAlture = _camFollow.FollowOffset;
        _camFollow.FollowOffset.y = defaultAlture.y;
    }


    void Update()
    {
        if (Mathf.Abs(player.lookInput.y) > 0.1f)
        {
            _returnTimer = waitBeforeReturn;
        }
        if (_returnTimer > 0)
        {
            _currentYOffset += player.lookInput.y * sensitivity * Time.deltaTime;
            _currentYOffset = Mathf.Clamp(_currentYOffset, 0, 7);
            _returnTimer -= Time.deltaTime;
        }
        else
        {
            _currentYOffset = Mathf.Lerp(_currentYOffset, defaultAlture.y, returnSpeed * Time.deltaTime);
        }

        _camFollow.FollowOffset.y = _currentYOffset;
        //_camFollow.FollowOffset.y += player.lookInput.y * sensitivity * Time.deltaTime;
        //_offsetExtension.Offset.y = _currentYOffset;
    }
}
