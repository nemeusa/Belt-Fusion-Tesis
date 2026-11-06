using PopupFramework.Popups;

namespace PopupFramework.Demo.Confirmation
{
    public class ConfirmationNoButtonTextBinder : PopupTextBinder<ConfirmationPopupData>
    {
        protected override string GetText(ConfirmationPopupData data) => data.noLabel;
    }
}
