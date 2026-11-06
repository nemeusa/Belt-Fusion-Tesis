using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MandoMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Selectable firstSelected;
    [Tooltip("UI/Cancel del Input System (Círculo).")]
    [SerializeField] private InputActionReference cancelAction;
    [Tooltip("Opcional: botón para abrir el menú (ej. Start / Options).")]
    [SerializeField] private InputActionReference openAction;

    private GameObject _selectionBeforeOpen;
    private GameObject _lastSelected;

    private void OnEnable()
    {
        cancelAction.action.Enable();
        cancelAction.action.performed += OnCancel;

        //if (openAction != null)
        //{
        //    openAction.action.Enable();
        //    openAction.action.performed += OnOpen;
        //}
    }

    private void OnDisable()
    {
        cancelAction.action.performed -= OnCancel;
        //if (openAction != null)
        //    openAction.action.performed -= OnOpen;
    }

    private void Update()
    {
        if (!panel.activeSelf) return;

        //if (!MainMenu.instance.inputLocked) MainMenu.instance.inputLocked = true;

        EventSystem es = EventSystem.current;
        GameObject current = es.currentSelectedGameObject;

        if (current != null)
            _lastSelected = current;
        else
            // Si se perdió el foco (click del mouse en vacío, dropdown cerrado, etc.), se recupera.
            es.SetSelectedGameObject(_lastSelected != null && _lastSelected.activeInHierarchy
                ? _lastSelected
                : firstSelected.gameObject);
    }

    private void OnOpen(InputAction.CallbackContext _)
    {
        if (!panel.activeSelf) Open();
    }

    private void OnCancel(InputAction.CallbackContext _)
    {
        if (!panel.activeSelf) return;
        if (IsAnyDropdownExpanded()) return; // El dropdown se cierra solo; el menú queda abierto

        Close();
    }

    public void Open()
    {
        _selectionBeforeOpen = EventSystem.current.currentSelectedGameObject;
        //panel.SetActive(true);
        _lastSelected = firstSelected.gameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }

    public void Close()
    {
        //panel.SetActive(false);
        //MainMenu.instance.inputLocked = false;
        EventSystem.current.SetSelectedGameObject(_selectionBeforeOpen);
    }

    private bool IsAnyDropdownExpanded()
    {
        foreach (var dropdown in panel.GetComponentsInChildren<TMP_Dropdown>(false))
            if (dropdown.IsExpanded) return true;

        return false;
    }
}
