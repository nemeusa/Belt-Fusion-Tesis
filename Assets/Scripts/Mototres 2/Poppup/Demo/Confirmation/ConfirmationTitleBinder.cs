using PopupFramework.Popups;

namespace PopupFramework.Demo.Confirmation
{
    public class ConfirmationTitleBinder : PopupTextBinder<ConfirmationPopupData>
    {
        protected override string GetText(ConfirmationPopupData data) => data.title;
    }
}