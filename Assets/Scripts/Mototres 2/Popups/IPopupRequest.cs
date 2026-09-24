namespace PopupFramework.Popups
{
    /// <summary>
    /// Handle no genérico para un popup pendiente. Permite que la cola
    /// guarde pedidos de *cualquier* tipo de dato sin conocer TData en
    /// tiempo de compilación, y que además pueda mostrarlo y ocultarlo sin
    /// saber de qué tipo es.
    /// </summary>
    public interface IPopupRequest
    {
        /// <summary>Muestra el popup de este pedido resolviendo su IPopupService&lt;TData&gt; y llamando Show.</summary>
        void Dispatch();

        /// <summary>Oculta el popup de este pedido resolviendo su IPopupService&lt;TData&gt; y llamando Hide.</summary>
        void RequestHide();
    }
}
