using PopupFramework.Core;
using PopupFramework.Demo.Confirmation;
using PopupFramework.Demo.Rewards;
using PopupFramework.Popups;
using UnityEngine;

namespace PopupFramework.Demo
{
    public class DemoTrigger : MonoBehaviour
    {
        [ContextMenu("ShowRewardPopup")]
        public void ShowRewardExample()
        {
            if (!ServiceLocator.Instance.TryGetDependency(out IPopupQueue queue))
            {
                Debug.LogWarning("IPopupQueue not found in the scene yet.");
                return;
            }

            queue.Show(new RewardPopupData
            {
                title = "New reward!",
                rewardName = "Double Jump",
                details = "Jump again while airborne.",
                buttonAString = "Equip",
                optionACallback = () => Debug.Log("Equip clicked"),
                buttonBString = "Later",
                optionBCallback = () => Debug.Log("Later clicked"),
            });
        }

        [ContextMenu("ShowConfirmationPopup")]
        public void ShowConfirmationExample()
        {
            if (!ServiceLocator.Instance.TryGetDependency(out IPopupQueue queue))
            {
                Debug.LogWarning("IPopupQueue not found in the scene yet.");
                return;
            }

            //1. Pedimos mostrar los siguientes datos en un popup
            queue.Show(new ConfirmationPopupData
            {
                title = "Queres reemplazar el arma?",
                message = "Discard changes?",
                yesLabel = "Reemplazar",
                noLabel = "Cancelar",
                onYes = () => Debug.Log("Discarded"),
                onNo = () => Debug.Log("Kept editing"),
            });
        }
    }
}
