using System;
using UnityEngine;
using UnityEngine.UI;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Asocia un Button a una Action opcional del dato del popup: se oculta
    /// si esa acción no está (ej. un popup con una sola opción), e
    /// invoca+limpia el callback una sola vez por click para que un doble
    /// click rápido no lo dispare dos veces.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class PopupActionBinder<TData> : PopupBinder<TData> where TData : class, IPopupData
    {
        Button _linkedButton;
        protected Button LinkedButton => _linkedButton = _linkedButton ? _linkedButton : GetComponent<Button>();

        Action _cachedCallback;

        protected override void Awake()
        {
            base.Awake();
            LinkedButton.onClick.AddListener(HandleClick);
        }

        protected sealed override void OnPopupUpdated(TData data)
        {
            _cachedCallback = data != null ? GetCallback(data) : null;
            gameObject.SetActive(_cachedCallback != null);
        }

        void HandleClick()
        {
            if (_cachedCallback == null) return;

            var callback = _cachedCallback;
            _cachedCallback = null;
            callback.Invoke();
            AfterInvoke();
        }

        /// <summary>
        /// Devuelve el callback que debe disparar este botón para el dato dado, o null para ocultarlo.
        /// El callback lo obtiene el hijo concreto que sabe QUE dato obtener entre todos los que hay.
        /// </summary>
        protected abstract Action GetCallback(TData data);

        /// <summary>Hook llamado justo después de ejecutar el callback. No hace nada por defecto.</summary>
        protected virtual void AfterInvoke() { }
    }
}
