using System.Collections.Generic;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Implementación por defecto de IPopupQueue: una cola FIFO simple que
    /// muestra un popup a la vez, sin importar su tipo de dato, y sabe
    /// cuál es el pedido actualmente activo para poder ocultarlo sin que
    /// quien llama Hide() tenga que decir de qué tipo es. Es una clase
    /// plana (sin MonoBehaviour ni singleton) para poder registrarla donde
    /// el proyecto arme sus servicios, y reemplazarla por otra IPopupQueue
    /// (por ejemplo con prioridades) sin tocar ningún popup ni binder.
    /// </summary>
    public sealed class PopupQueue : IPopupQueue
    {
        readonly Queue<IPopupRequest> _pending = new();
        IPopupRequest _current;

        //2. Se recibe el dato y se crea un wrapper donde se guarda el tipo de popup data
        //y se encarga de llamar al servicio que maneja ese tipo de popup
        public void Show<TData>(TData data) where TData : class, IPopupData
        {
            //Excepcion a la queue: si el pedido activa es del mismo tipo (aunque se este cerrando),
            //es el mismo popup pidiendo volver a mostrarse y por ende se le pide activarse directamente
            if (_current is PopupRequest<TData>)
            {
                _current = new PopupRequest<TData>(data);
                _current.Dispatch();
                return;
            }

            _pending.Enqueue(new PopupRequest<TData>(data));
            TryShowNext();
        }

        public void Hide()
        {
            _current?.RequestHide();
        }

        public void NotifyClosed()
        {
            _current = null;
            TryShowNext();
        }

        void TryShowNext()
        {
            if (_current != null || _pending.Count == 0) return;

            _current = _pending.Dequeue();
            _current.Dispatch();
        }
    }
}
