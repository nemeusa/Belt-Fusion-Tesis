using PopupFramework.Popups;
using UnityEngine;

namespace PopupFramework.Demo.Rewards
{
    public class RewardSpriteBinder : PopupImageBinder<RewardPopupData>
    {
        protected override Sprite GetSprite(RewardPopupData data) => data.sprite;
    }
}
