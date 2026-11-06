using PopupFramework.Popups;

namespace PopupFramework.Demo.Confirmation
{
    public class ConfirmationYesButtonTextBinder : PopupTextBinder<ConfirmationPopupData>
    {
        protected override string GetText(ConfirmationPopupData data) => data.yesLabel;
    }
}
