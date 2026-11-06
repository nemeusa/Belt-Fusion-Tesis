using PopupFramework.Popups;
using UnityEngine;

namespace PopupFramework.Demo.Rewards
{
    /// <summary>
    /// Popup concreto: lo único específico de reward es "qué hacer al mostrarse"
    /// El resto (registrarse como servicio, prender/apagar el
    /// Canvas, propagar el dato a los binders) se hereda de PopupView.
    /// Cambiá este Debug.Log por lo que tu proyecto use para sonido/haptics/
    /// analytics
    /// La gracia de OnShow/OnHide es que esa decisión vive en un solo lugar.
    /// </summary>
    public class RewardPopupView : PopupView<RewardPopupData>
    {
        protected override void OnShow(RewardPopupData data)
        {
            Debug.Log($"[RewardPopup] shown: {data.rewardName}");
        }
    }
}
