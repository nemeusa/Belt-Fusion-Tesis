using TMPro;
using UnityEngine;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Base para cualquier texto que muestra un campo del dato del popup.
    /// Cada tipo de popup solo dice QUE campo.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public abstract class PopupTextBinder<TData> : PopupBinder<TData> where TData : class, IPopupData
    {
        TMP_Text _linkedText;
        protected TMP_Text LinkedText => _linkedText = _linkedText ? _linkedText : GetComponent<TMP_Text>();

        protected sealed override void OnPopupUpdated(TData data)
        {
            LinkedText.text = data != null ? GetText(data) : string.Empty;
        }

        /// <summary>Devuelve el texto que debe mostrar este label para el dato dado.</summary>
        protected abstract string GetText(TData data);
    }
}
