using System;
using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardOptionBButtonBinder : PopupActionBinder<RewardPopupData>
    {
        protected override Action GetCallback(RewardPopupData data) => data.optionBCallback;
    }
}
