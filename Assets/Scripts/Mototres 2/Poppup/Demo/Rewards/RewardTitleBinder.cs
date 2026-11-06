using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardTitleBinder : PopupTextBinder<RewardPopupData>
    {
        protected override string GetText(RewardPopupData data) => data.title;
    }
}
