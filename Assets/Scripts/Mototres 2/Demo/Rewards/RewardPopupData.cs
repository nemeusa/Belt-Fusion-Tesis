using System;
using PopupFramework.Popups;
using UnityEngine;

namespace PopupFramework.Demo.Rewards
{
    /// <summary>
    /// Dato puro del popup de recompensa. Sin MonoBehaviour, sin
    /// referencias de UI. Es lo único específico de "reward" que el resto
    /// del juego necesita conocer para disparar este popup.
    /// </summary>
    public class RewardPopupData : IPopupData
    {
        public string title;
        public string rewardName;
        public Sprite sprite;
        public string details;
        public string buttonAString;
        public Action optionACallback;
        public string buttonBString;
        public Action optionBCallback;
    }
}
