namespace PopupFramework.Popups
{
    /// <summary>
    /// Muestra/oculta este GameObject según una condición sobre el dato del popup.
    /// Util para secciones opcionales que deben colapsar si están vacías
    /// (un bloque de detalles, un footer, una badge).
    /// </summary>
    public abstract class PopupVisibilityBinder<TData> : PopupBinder<TData> where TData : class, IPopupData
    {
        protected sealed override void OnPopupUpdated(TData data)
        {
            gameObject.SetActive(data != null && IsVisible(data));
        }

        /// <summary>Devuelve si este GameObject debe estar activo para el dato dado.</summary>
        protected abstract bool IsVisible(TData data);
    }
}
