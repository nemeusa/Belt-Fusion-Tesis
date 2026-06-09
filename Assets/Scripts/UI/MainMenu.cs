using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.UI;

public class MainMenu : MonoBehaviour
{
    private InputSystemUIInputModule _uiModule;
    private InputAction _cancelAction;

    public GameObject canvasOne;
    public GameObject canvasTwo;

    public GameObject primerBoton;
    public GameObject primerLevelBoton;

    public AudioMixer mixer;

    bool isMuted = false;

    void Start()
    {
        Time.timeScale = 1;
        // Buscamos el componente de la foto
        _uiModule = FindFirstObjectByType<InputSystemUIInputModule>();

        // Obtenemos la acción que está mapeada en el casillero "Cancel"
        if (_uiModule != null)
        {
            _cancelAction = _uiModule.cancel.action;
        }
    }

    void Update()
    {
        // 1. Lógica de Selección Automática para Mando
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (canvasTwo.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(primerLevelBoton);
            }
            else if (canvasOne.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(primerBoton);
            }
        }

        if (_cancelAction != null && _cancelAction.WasPressedThisFrame())
        {
            if (canvasTwo.activeSelf)
            {
                VolverAlMenuPrincipal();
            }
        }

    }

    // Llamá a esta función desde el botón "Play" del menú principal
    public void IrASeleccionNiveles()
    {
        canvasOne.SetActive(false);
        canvasTwo.SetActive(true);

        // Forzamos la selección del primer nivel inmediatamente
        EventSystem.current.SetSelectedGameObject(primerLevelBoton);
    }

    public void VolverAlMenuPrincipal()
    {
        canvasTwo.SetActive(false);
        canvasOne.SetActive(true);

        // Forzamos la selección del botón play de vuelta
        EventSystem.current.SetSelectedGameObject(primerBoton);
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Mute(string nameG)
    {
        isMuted = !isMuted;

        if (isMuted)
            mixer.SetFloat(nameG, -80);
        else
            mixer.SetFloat(nameG, 0);
    }


    public void OffIcon(GameObject offIcon)
    {
        if (isMuted)
            offIcon.SetActive(true);

        else
            offIcon.SetActive(false);
    }

}
