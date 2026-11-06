using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardNameBinder : PopupTextBinder<RewardPopupData>
    {
        protected override string GetText(RewardPopupData data) => data.rewardName;
    }
}
