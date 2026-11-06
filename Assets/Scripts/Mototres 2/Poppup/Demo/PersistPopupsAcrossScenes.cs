using UnityEngine;

namespace PopupFramework.Demo
{
    /// <summary>
    /// Conveniencia opcional para una escena de bootstrap: ponelo en el
    /// GameObject padre que contiene todos tus canvases de popups
    /// persistentes (RewardPopup, ConfirmationPopup...) y sobrevive el
    /// cambio de escena, así todo PopupView de abajo ya registró su
    /// IPopupService antes de que el menú principal (o cualquier
    /// otra escena) cargue y empiece a pedirlo.
    ///
    /// Este es el "para qué" de una escena de bootstrap inicial: no es por
    /// los popups en sí, es por controlar CUANDO se registran.
    /// </summary>
    public class PersistPopupsAcrossScenes : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
