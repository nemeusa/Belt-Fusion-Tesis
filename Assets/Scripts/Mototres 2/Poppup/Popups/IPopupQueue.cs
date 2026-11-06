namespace PopupFramework.Popups
{
    
    public interface IPopupQueue
    {
        /// <summary>Arma el IPopupRequest del dato dado y lo encola.</summary>
        void Show<TData>(TData data) where TData : class, IPopupData;

        /// <summary>Empieza a ocultar el popup actualmente activo, si hay alguno. No hace nada si no hay ninguno mostrándose.</summary>
        void Hide();

        /// <summary>
        /// Llamar cuando el popup activo terminó de cerrarse de verdad
        /// (lo hace PopupView internamente)
        /// Muestra el siguiente de la cola si hay alguno.
        /// </summary>
        void NotifyClosed();
    }
}
