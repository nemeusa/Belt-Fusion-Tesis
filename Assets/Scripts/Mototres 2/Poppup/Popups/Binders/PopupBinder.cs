using UnityEngine;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Compartida por cualquier componente que reacciona al dato de
    /// un popup. Busca el IPopupProvider hacia el parent en la
    /// jerarquía una sola vez y se suscribe/desuscribe. Los binders
    /// concretos (PopupTextBinder, PopupImageBinder...) solo implementan
    /// OnPopupUpdated con la línea de UI que les toca.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class PopupBinder<TData> : MonoBehaviour where TData : class, IPopupData
    {
        IPopupProvider<TData> _provider;

        protected virtual void Awake()
        {
            _provider = GetComponentInParent<IPopupProvider<TData>>();
            _provider.OnUpdatedPopup += OnPopupUpdated;
        }

        protected virtual void OnDestroy()
        {
            if (_provider is null) return;

            _provider.OnUpdatedPopup -= OnPopupUpdated;
        }

        /// <summary>
        /// Se llama con el dato actual del popup cada vez que cambia. Los
        /// binders deberían tolerar null por las dudas, aunque un popup en
        /// uso normalmente no lo envía.
        /// </summary>
        protected abstract void OnPopupUpdated(TData data);
    }
}
