using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private InputSystemUIInputModule _uiModule;
    private InputAction _cancelAction;

    //public GameObject canvasOne;
    //public GameObject canvasTwo;

    //public GameObject primerBoton;
    //public GameObject primerLevelBoton;

    [System.Serializable]
    public class MenuPanel
    {
        public string panelID; // Nombre identificador (ej: "Principal", "Niveles", "Opciones")
        public GameObject panelObject;
        public GameObject firstSelectedButton;
    }

    [Header("Configuración de Paneles")]
    public List<MenuPanel> panels = new List<MenuPanel>();
    public int defaultPanelIndex = 0; // Índice del menú principal de inicio

    private Stack<MenuPanel> _panelHistory = new Stack<MenuPanel>();

    public AudioMixer mixer;
    bool isMuted = false;


    Animator nextAni;


    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Time.timeScale = 1;
    //    // Buscamos el componente de la foto
    //    _uiModule = FindFirstObjectByType<InputSystemUIInputModule>();

    //    // Obtenemos la acción que está mapeada en el casillero "Cancel"
    //    if (_uiModule != null)
    //    {
    //        _cancelAction = _uiModule.cancel.action;
    //    }
    //}

    //void Update()
    //{
    //    // 1. Lógica de Selección Automática para Mando
    //    if (EventSystem.current.currentSelectedGameObject == null)
    //    {
    //        if (canvasTwo.activeSelf)
    //        {
    //            EventSystem.current.SetSelectedGameObject(primerLevelBoton);
    //        }
    //        else if (canvasOne.activeSelf)
    //        {
    //            EventSystem.current.SetSelectedGameObject(primerBoton);
    //        }
    //    }

    //    if (_cancelAction != null && _cancelAction.WasPressedThisFrame())
    //    {
    //        if (canvasTwo.activeSelf)
    //        {
    //            VolverAlMenuPrincipal();
    //        }
    //    }

    //}

    //// Llamá a esta función desde el botón "Play" del menú principal
    //public void IrASeleccionNiveles()
    //{
    //    canvasOne.SetActive(false);
    //    canvasTwo.SetActive(true);

    //    // Forzamos la selección del primer nivel inmediatamente
    //    EventSystem.current.SetSelectedGameObject(primerLevelBoton);
    //}

    //public void VolverAlMenuPrincipal()
    //{
    //    canvasTwo.SetActive(false);
    //    canvasOne.SetActive(true);

    //    // Forzamos la selección del botón play de vuelta
    //    EventSystem.current.SetSelectedGameObject(primerBoton);
    //}

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;

        _uiModule = FindFirstObjectByType<InputSystemUIInputModule>();
        if (_uiModule != null)
        {
            _cancelAction = _uiModule.cancel.action;
        }

        // Apagamos todos los paneles al arrancar
        foreach (var panel in panels)
        {
            if (panel.panelObject != null)
                panel.panelObject.SetActive(false);
        }

        // Abrimos el panel inicial si existe
        if (panels.Count > defaultPanelIndex)
        {
            OpenPanel(defaultPanelIndex);
        }
    }

    void Update()
    {
        // 1. Recuperar el foco del mando si se pierde al hacer clic en zonas vacías
        if (EventSystem.current.currentSelectedGameObject == null && _panelHistory.Count > 0)
        {
            MenuPanel currentPanel = _panelHistory.Peek();
            if (currentPanel != null && currentPanel.firstSelectedButton != null)
            {
                EventSystem.current.SetSelectedGameObject(currentPanel.firstSelectedButton);
            }
        }

        // 2. Volver atrás con el botón de Cancelar (Círculo / B / Esc)
        if (_cancelAction != null && _cancelAction.WasPressedThisFrame())
        {
            GoBack();
        }
    }

    // Abre un panel mediante su número de índice en la lista del Inspector
    public void OpenPanel(int index)
    {
        if (index < 0 || index >= panels.Count) return;

        ActivatePanel(panels[index]);
    }

    // Abre un panel mediante su ID de texto
    public void OpenPanelByID(string id)
    {
      
        MenuPanel targetPanel = panels.Find(p => p.panelID.Equals(id, System.StringComparison.OrdinalIgnoreCase));
        if (targetPanel != null)
        {
            ActivatePanel(targetPanel);
        }
    }

    private void ActivatePanel(MenuPanel newPanel)
    {
        // Ocultar panel anterior si hay alguno activo
        if (_panelHistory.Count > 0)
        {
            _panelHistory.Peek().panelObject.SetActive(false);
        }

        // Mostrar nuevo panel y registrarlo en el historial
        newPanel.panelObject.SetActive(true);
        _panelHistory.Push(newPanel);

        // Forzar selección del botón principal
        EventSystem.current.SetSelectedGameObject(newPanel.firstSelectedButton);
    }

    // Vuelve al panel anterior desapilando el menú actual
    public void GoBack()
    {
        // Si estamos en el panel principal (raíz), no hace nada al presionar volver
        if (_panelHistory.Count <= 1) return;

        // Desactivar el panel actual
        MenuPanel currentPanel = _panelHistory.Pop();
        currentPanel.panelObject.SetActive(false);

        // Reactivar el panel anterior en el historial
        MenuPanel previousPanel = _panelHistory.Peek();
        previousPanel.panelObject.SetActive(true);

        // Restablecer foco del mando
        EventSystem.current.SetSelectedGameObject(previousPanel.firstSelectedButton);
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
