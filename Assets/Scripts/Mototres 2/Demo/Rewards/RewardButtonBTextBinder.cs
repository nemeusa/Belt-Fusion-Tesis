using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardButtonBTextBinder : PopupTextBinder<RewardPopupData>
    {
        protected override string GetText(RewardPopupData data) => data.buttonBString;
    }
}
