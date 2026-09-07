using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private InputSystemUIInputModule _uiModule;
    private InputAction _cancelAction;

    [Header("UI del Menú")]
    public GameObject panelPausaUI;
    public GameObject primerBotonPausa; // El botón "Reanudar"

    public bool juegoPausado = false;
    [SerializeField] PlayerController player;

    void Start()
    {
        // Copiado de tu MainMenu: Buscamos el componente de Input de la UI
        _uiModule = FindFirstObjectByType<InputSystemUIInputModule>();

        if (_uiModule != null)
        {
            _cancelAction = _uiModule.cancel.action;
        }

        // Nos aseguramos de que el panel arranque apagado al empezar el nivel
        panelPausaUI.SetActive(false);
    }

    void Update()
    {
        // 1. Lógica de Selección Automática para Mando (Copiada de tu MainMenu)
        // Si el juego está pausado y el EventSystem pierde el foco (por usar el mouse u otro input),
        // re-seleccionamos el botón por defecto para que el joystick no se quede trabado.
        if (juegoPausado)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(primerBotonPausa);
            }

            // 2. Volver atrás con el botón Cancel (B / Círculo / Escape) cambiado al estilo de tu MainMenu
            if (_cancelAction != null && _cancelAction.WasPressedThisFrame())
            {
                Reanudar();
            }
        }
    }

    // Esta función la podés llamar desde el GameManager.instance.PauseGame()
    // o directamente mapeando el Input del PlayerController
    public void AlternarPausa()
    {
        if (juegoPausado)
        {
            Reanudar();
        }
        else
        {
            Pausar();
        }
    }

    public void Pausar()
    {
        juegoPausado = true;
        panelPausaUI.SetActive(true);

        // Frenamos el tiempo del juego (físicas, corrutinas, etc.)
        Time.timeScale = 0f;

        // Desactivamos al player usando tu variable de PlayerController
        if (player != null)
        {
            player.dontMovePlayer = true;
            player.moveInput = Vector2.zero; // Reseteamos el input para que no se quede deslizando
        }

        // Forzamos la selección del primer botón (estilo tu IrASeleccionNiveles)
        EventSystem.current.SetSelectedGameObject(null); // Limpieza preventiva
        EventSystem.current.SetSelectedGameObject(primerBotonPausa);
    }

    public void Reanudar()
    {
        juegoPausado = false;
        panelPausaUI.SetActive(false);

        // Devolvemos el tiempo a la normalidad
        Time.timeScale = 1f;

        // Reactivamos al player
        if (player != null)
        {
            player.dontMovePlayer = false;
        }

        // Volvemos a bloquear el cursor como hacés en el Start de tu MainMenu
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void NextScene(string nombreMenu)
    {
        // REGLA DE ORO: Siempre timeScale en 1 antes de cambiar de escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreMenu);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameManager.instance.levelName);

    }
}

