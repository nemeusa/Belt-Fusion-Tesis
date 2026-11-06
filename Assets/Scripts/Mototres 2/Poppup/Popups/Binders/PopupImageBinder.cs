using UnityEngine;
using UnityEngine.UI;

namespace PopupFramework.Popups
{
    /// <summary>Base para cualquier Image que muestra un sprite del dato del popup.</summary>
    [RequireComponent(typeof(Image))]
    public abstract class PopupImageBinder<TData> : PopupBinder<TData> where TData : class, IPopupData
    {
        Image _linkedImage;
        protected Image LinkedImage => _linkedImage = _linkedImage ? _linkedImage : GetComponent<Image>();

        protected sealed override void OnPopupUpdated(TData data)
        {
            if (data == null) return;

            LinkedImage.sprite = GetSprite(data);
        }

        /// <summary>Devuelve el sprite que debe mostrar esta imagen para el dato dado.</summary>
        protected abstract Sprite GetSprite(TData data);
    }
}
