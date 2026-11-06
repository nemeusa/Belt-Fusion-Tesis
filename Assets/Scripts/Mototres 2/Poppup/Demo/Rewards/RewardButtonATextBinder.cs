using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardButtonATextBinder : PopupTextBinder<RewardPopupData>
    {
        protected override string GetText(RewardPopupData data) => data.buttonAString;
    }
}
