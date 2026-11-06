using PopupFramework.Core;
using UnityEngine;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Envuelve un dato de popup tipado para que viaje por la IPopupQueue
    /// como IPopupRequest, y se despacha solo resolviendo el
    /// IPopupService correspondiente en el ServiceLocator cuando le
    /// toca su turno.
    /// </summary>
    public readonly struct PopupRequest<TData> : IPopupRequest where TData : class, IPopupData
    {
        readonly TData _data;

        public PopupRequest(TData data)
        {
            _data = data;
        }

        //3. Busca el servicio encargado de manejar este tipo de popup data y pide mostrar el canvas pasando los datos a utilizar
        public void Dispatch()
        {
            if (ServiceLocator.Instance.TryGetDependency(out IPopupService<TData> popupService))
            {
                popupService.Show(_data);
            }
            else
            {
                Debug.LogWarning($"No IPopupService<{typeof(TData).Name}> registered. Is its PopupView in the scene yet?");
            }
        }

        public void RequestHide()
        {
            if (ServiceLocator.Instance.TryGetDependency(out IPopupService<TData> popupService))
                popupService.Hide();
        }
    }
}
