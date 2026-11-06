using System;
using PopupFramework.Popups;

namespace PopupFramework.Demo.Confirmation
{
    public class ConfirmationYesActionBinder : PopupActionAndCloseBinder<ConfirmationPopupData>
    {
        protected override Action GetCallback(ConfirmationPopupData data) => data.onYes;
    }
}
