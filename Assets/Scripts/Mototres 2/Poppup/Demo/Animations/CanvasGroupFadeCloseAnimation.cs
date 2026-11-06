using System;
using System.Collections;
using PopupFramework.Popups;
using UnityEngine;

namespace PopupFramework.Demo.Animations
{
    /// <summary>
    /// IPopupCloseAnimation: baja el alpha de un CanvasGroup a 0 en
    /// '_duration' segundos y bloquea el input mientras dura, después avisa
    /// que terminó. Se agrega junto a un PopupView, en un
    /// GameObject que también tenga un CanvasGroup (normalmente el mismo que tiene el Canvas).
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFadeCloseAnimation : MonoBehaviour, IPopupCloseAnimation
    {
        [SerializeField] float _duration = 0.25f;

        CanvasGroup _canvasGroup;
        CanvasGroup LinkedCanvasGroup => _canvasGroup = _canvasGroup ? _canvasGroup : GetComponent<CanvasGroup>();

        Coroutine _running;

        public void Play(Action onComplete)
        {
            _running = StartCoroutine(FadeOut(onComplete));
        }

        public void Cancel()
        {
            if (_running != null) StopCoroutine(_running);
            _running = null;

            // Un Show() interrumpió el cierre: el popup vuelve a estar en
            // pantalla, así que se restaura a "totalmente visible" ya mismo
            // en vez de dejarlo a mitad de fade.
            LinkedCanvasGroup.alpha = 1f;
            LinkedCanvasGroup.interactable = true;
            LinkedCanvasGroup.blocksRaycasts = true;
        }

        IEnumerator FadeOut(Action onComplete)
        {
            LinkedCanvasGroup.interactable = false;
            LinkedCanvasGroup.blocksRaycasts = false;

            var startAlpha = LinkedCanvasGroup.alpha;
            var elapsed = 0f;

            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                LinkedCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / _duration);
                yield return null;
            }

            // Se resetea para la próxima vez que se muestre este popup
            // PopupView.Show solo reactiva el Canvas, no sabe que esta
            // animación tocó el alpha.
            LinkedCanvasGroup.alpha = startAlpha > 0f ? startAlpha : 1f;
            LinkedCanvasGroup.interactable = true;
            LinkedCanvasGroup.blocksRaycasts = true;

            _running = null;
            onComplete();
        }
    }
}
