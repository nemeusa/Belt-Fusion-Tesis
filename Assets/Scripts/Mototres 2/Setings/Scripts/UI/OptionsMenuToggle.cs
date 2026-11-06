using UnityEngine;
using UnityEngine.InputSystem;

namespace GameOptions
{
    /// <summary>
    /// Abre y cierra el menu con una accion del Input System (ej. Escape) y
    /// maneja el cursor: libre con el menu abierto, bloqueado con el menu cerrado.
    /// En un menu principal (sin gameplay) no hace falta: basta un boton que llame a OptionsMenu.Open().
    /// </summary>
    public sealed class OptionsMenuToggle : MonoBehaviour
    {
        [SerializeField] private OptionsMenu menu;
        [SerializeField] private InputActionReference toggleAction;

        [Tooltip("Bloquear y ocultar el cursor mientras el menu esta cerrado (juegos en primera persona).")]
        [SerializeField] private bool lockCursorWhenClosed = true;

        private void OnEnable()
        {
            toggleAction.action.performed += OnTogglePerformed;
            toggleAction.action.Enable();
            menu.VisibilityChanged += UpdateCursor;
        }

        private void OnDisable()
        {
            toggleAction.action.performed -= OnTogglePerformed;
            menu.VisibilityChanged -= UpdateCursor;
        }

        // En Start (y no en OnEnable) porque OptionsMenu cierra su panel en Awake.
        private void Start() => UpdateCursor(menu.IsOpen);

        private void OnTogglePerformed(InputAction.CallbackContext _)
        {
            if (menu.IsOpen)
                menu.Close();
            else
                menu.Open();
        }

        private void UpdateCursor(bool menuOpen)
        {
            if (!lockCursorWhenClosed)
                return;

            Cursor.lockState = menuOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = menuOpen;
        }
    }
}
