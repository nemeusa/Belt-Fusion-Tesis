using PopupFramework.Popups;

namespace PopupFramework.Demo.Confirmation
{
    public class ConfirmationMessageBinder : PopupTextBinder<ConfirmationPopupData>
    {
        protected override string GetText(ConfirmationPopupData data) => data.message;
    }
}
