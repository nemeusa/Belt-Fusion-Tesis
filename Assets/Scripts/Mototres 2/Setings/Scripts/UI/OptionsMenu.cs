using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameOptions
{
    /// <summary>
    /// Coordina el menu con el flujo "Aplicar / Cancelar":
    ///  - Abrir: las secciones muestran la configuracion aplicada.
    ///  - Editar: los cambios viven solo en los controles de la UI (todavia no afectan al juego).
    ///  - Aplicar: se copia la configuración actual, cada seccion vuelca lo suyo y se envia al manager.
    ///  - Cancelar: se cierra sin aplicar, al reabrir se vuelve a mostrar lo aplicado.
    ///  - Restaurar: se cargan los valores por defecto en los controles (hay que Aplicar para confirmarlos).
    ///
    /// Este script debe estar en un objeto que permanezca activo (ej. el Canvas),
    /// con el panel del menu y las secciones como hijos.
    /// </summary>
    public sealed class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button applyButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button resetButton;

        [SerializeField] private InputActionReference applyAction;
        [SerializeField] private InputActionReference cancelAction;
        [SerializeField] private InputActionReference resetAction;


        public bool IsOpen => panel.activeSelf;

        /// <summary>Se dispara al abrir (true) o cerrar (false) el menu.</summary>
        public event Action<bool> VisibilityChanged;

        private ISettingsSection[] _sections;

        private void Awake()
        {
            _sections = GetComponentsInChildren<ISettingsSection>(includeInactive: true);

            //applyButton.onClick.AddListener(ApplyChanges);
            //cancelButton.onClick.AddListener(Close);
            //resetButton.onClick.AddListener(ResetToDefaults);

            panel.SetActive(false);
        }

        private void OnEnable()
        {
            applyAction.action.Enable();
            cancelAction.action.Enable();
            resetAction.action.Enable();

            applyAction.action.performed += ApplyChanges;
            cancelAction.action.performed += CloseInput;
            resetAction.action.performed += ResetToDefaults;

     
        }

        private void OnDisable()
        {
            applyAction.action.performed -= ApplyChanges;
            cancelAction.action.performed -= CloseInput;
            resetAction.action.performed -= ResetToDefaults;

        }


        public void Open()
        {
            // Primero se activa el panel: asi cada seccion ejecuta su Awake (arma sus listas)
            // antes de recibir los datos.
            panel.SetActive(true);
            LoadIntoSections(SettingsManager.Instance.Current);
            VisibilityChanged?.Invoke(true);
        }

        private void CloseInput(InputAction.CallbackContext _) => Close();

        public void Close()
        {
            panel.SetActive(false);
            VisibilityChanged?.Invoke(false);
        }

        private void ApplyChanges(InputAction.CallbackContext _)
        {
            GameSettings chosen = SettingsManager.Instance.Current.Clone();

            foreach (ISettingsSection section in _sections)
                section.WriteTo(chosen);

            Debug.Log("Se aplico los cambios");
            SettingsManager.Instance.Apply(chosen);
        }

        private void ResetToDefaults(InputAction.CallbackContext _) => LoadIntoSections(GameSettings.CreateDefault());

        private void LoadIntoSections(GameSettings settings)
        {
            foreach (ISettingsSection section in _sections)
                section.Load(settings);
        }
    }
}
