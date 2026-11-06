using System;
using PopupFramework.Popups;

namespace PopupFramework.Demo.Confirmation
{
    /// <summary>
    /// Segundo tipo de popup, sin relación con RewardPopupData pero sobre el
    /// mismo framework. Esto es en lo que se convierte un panel de
    /// confirmación con singleton hardcodeado una vez desacoplado: dato
    /// puro, sin MonoBehaviour, sin panel fijo, sin Instance estático.
    /// </summary>
    public class ConfirmationPopupData : IPopupData
    {
        public string title;
        public string message;
        public string yesLabel;
        public string noLabel;
        public Action onYes;
        public Action onNo;
    }
}
