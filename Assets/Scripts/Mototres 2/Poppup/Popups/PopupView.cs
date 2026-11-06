using PopupFramework.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Clase base de la "root" de cualquier canvas popup: se registra como
    /// IPopupService en el ServiceLocator, prende/apaga su Canvas, y
    /// pasa su dato por OnUpdatedPopup para que los binders hijos
    /// reaccionen cada uno a su campo.
    ///
    /// Hide() soporta animación de cierre opcional: si el GameObject tiene
    /// un IPopupCloseAnimation, Hide() la reproduce y recién desactiva el
    /// Canvas cuando termina. Sin ella, Hide() es inmediato y a nadie de
    /// afuera le importa cuál de los dos casos es.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    public abstract class PopupView<TData> : MonoBehaviour, IPopupService<TData>, IPopupProvider<TData>
        where TData : class, IPopupData
    {
        public event Action<TData> OnUpdatedPopup;

        Canvas _canvas;

        IPopupCloseAnimation _closeAnimation;

        bool _isClosing;
        int _closeToken;

        protected virtual void Awake()
        {
            ServiceLocator.Instance.RegisterDependency<IPopupService<TData>>(this);
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
            
            _closeAnimation = GetComponent<IPopupCloseAnimation>();
        }

        protected virtual void OnDestroy()
        {
            ServiceLocator.Instance.RemoveDependency<IPopupService<TData>>(this);
        }

        
        public void Show(TData data)
        {
            _closeToken++;

            // Si había un cierre en curso hay que cortar la animación
            if (_isClosing)
                _closeAnimation?.Cancel();

            _isClosing = false;

            // Si el botón que abrió/cerró este popup había quedado marcado
            // como "seleccionado" por el EventSystem, se ve resaltado apenas
            // el popup reaparece.
            EventSystem.current?.SetSelectedGameObject(null);

            _canvas.enabled = true;
            OnShow(data);
            OnUpdatedPopup?.Invoke(data);
        }

        /// <summary>
        /// Empieza a cerrar el popup. Si hay animación de cierre, el Canvas
        /// sigue activo hasta que la animación termina.
        /// Recién ahí se desactiva de verdad y avanza la cola, si hay.
        /// </summary>
        public void Hide()
        {
            if (!_canvas.enabled || _isClosing) return;

            _isClosing = true;
            var token = ++_closeToken;

            if (_closeAnimation != null)
                _closeAnimation.Play(() => FinishHide(token));
            else
                FinishHide(token);
        }

        void FinishHide(int token)
        {
            // Hubo un Show()/Hide() más nuevo desde que empezó esta
            // animación, se ignora este final viejo para no ocultar un
            // popup que ya se volvió a mostrar (o que ya cerró de otra forma).
            if (token != _closeToken) return;

            _isClosing = false;
            _canvas.enabled = false;
            OnHide();

            if (ServiceLocator.Instance.TryGetDependency(out IPopupQueue queue))
                queue.NotifyClosed();
        }

        /// <summary>Hook para efectos al mostrar el popup (sonido, haptics, analytics...). No hace nada por defecto.</summary>
        protected virtual void OnShow(TData data)
        {
        }

        /// <summary>Hook para efectos cuando el popup terminó de ocultarse (después de la animación, si hay). No hace nada por defecto.</summary>
        protected virtual void OnHide()
        {
        }
    }
}
