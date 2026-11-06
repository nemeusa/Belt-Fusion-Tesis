using System;
using PopupFramework.Popups;

namespace PopupFramework.Demo.Confirmation
{
    public class ConfirmationNoActionBinder : PopupActionAndCloseBinder<ConfirmationPopupData>
    {
        protected override Action GetCallback(ConfirmationPopupData data) => data.onNo;
    }
}
