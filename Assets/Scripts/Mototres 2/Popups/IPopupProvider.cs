using System;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Lado "lectura" de un popup: lo expone quien tiene el dato actual (un
    /// PopupView) para que sus binders hijos reaccionen.
    /// Separado a propósito de IPopupService (lado "escritura") para que un binder
    /// nunca pueda llamar Show/Hide, y quien solo quiere mostrar un popup
    /// nunca necesite ver este evento (ISP).
    /// </summary>
    public interface IPopupProvider<TData> where TData : class, IPopupData
    {
        /// <summary>Se dispara cada vez que cambia el dato del popup.</summary>
        event Action<TData> OnUpdatedPopup;
    }
}
