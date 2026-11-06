using PopupFramework.Core;
using UnityEngine;

namespace PopupFramework.Popups
{
    /// <summary>
    /// Registra el IPopupQueue por defecto en el ServiceLocator al
    /// arrancar. Poné una instancia de esto en tu escena de bootstrap
    /// </summary>
    public class PopupQueueInstaller : MonoBehaviour
    {
        void Awake()
        {
            ServiceLocator.Instance.RegisterDependency<IPopupQueue>(new PopupQueue());
        }
    }
}
