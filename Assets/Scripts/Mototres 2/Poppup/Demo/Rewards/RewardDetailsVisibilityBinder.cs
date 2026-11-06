using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardDetailsVisibilityBinder : PopupVisibilityBinder<RewardPopupData>
    {
        protected override bool IsVisible(RewardPopupData data) => !string.IsNullOrEmpty(data.details);
    }
}
