using System;
using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardOptionAButtonBinder : PopupActionBinder<RewardPopupData>
    {
        protected override Action GetCallback(RewardPopupData data) => data.optionACallback;
    }
}
