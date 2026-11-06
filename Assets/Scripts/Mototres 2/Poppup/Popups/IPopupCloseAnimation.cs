using System;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Strategy opcional para reproducir una animación de cierre antes de
    /// desactivar el popup. Cualquier MonoBehaviour que la implemente en el
    /// mismo GameObject que un PopupView hace que Hide() espere a
    /// onComplete antes de apagar el Canvas y disparar OnHide().
    /// Si no se implementa, Hide() es inmediato.
    /// </summary>
    public interface IPopupCloseAnimation
    {
        /// <summary>
        /// Inicia la animación de cierre. Debe invocar onComplete una sola
        /// vez al terminar. Hide() no oculta el popup hasta entonces.
        /// </summary>
        void Play(Action onComplete);

        /// <summary>
        /// Corta la animación en curso (si hay una) y deja el popup en
        /// estado "totalmente visible": PopupView la llama cuando un Show()
        /// interrumpe un cierre que todavía estaba jugando, para que la
        /// animación no siga corriendo sola de fondo.
        /// </summary>
        void Cancel();
    }
}
