using PopupFramework.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Botón de cerrar genérico para cualquier popup: resuelve la
    /// IPopupQueue en el ServiceLocator y llama Hide(), que cierra el
    /// popup activo en ese momento, sea cual sea su tipo de dato. Ya no
    /// necesita un TData propio como antes: como todo popup se muestra a
    /// través de la queue, solo puede haber uno activo a la vez, así que
    /// "cerrar" nunca es ambiguo.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class ClosePopupOnClick : MonoBehaviour
    {
        Button _linkedButton;
        Button LinkedButton => _linkedButton = _linkedButton ? _linkedButton : GetComponent<Button>();

        void Awake()
        {
            LinkedButton.onClick.AddListener(HandleClick);
        }

        void HandleClick()
        {
            if (ServiceLocator.Instance.TryGetDependency(out IPopupQueue queue))
                queue.Hide();
        }
    }
}
