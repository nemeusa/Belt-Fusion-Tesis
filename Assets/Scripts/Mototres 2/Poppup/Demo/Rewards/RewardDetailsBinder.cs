using PopupFramework.Popups;

namespace PopupFramework.Demo.Rewards
{
    public class RewardDetailsBinder : PopupTextBinder<RewardPopupData>
    {
        protected override string GetText(RewardPopupData data) => data.details;
    }
}
