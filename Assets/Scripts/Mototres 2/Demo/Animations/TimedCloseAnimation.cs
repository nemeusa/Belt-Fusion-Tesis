using System;
using System.Collections;
using PopupFramework.Popups;
using UnityEngine;

namespace PopupFramework.Demo.Animations
{
    /// <summary>
    /// La IPopupCloseAnimation más simple posible: espera una duración fija
    /// antes de avisar que terminó. Sin efecto visual propio.
    /// Util como punto de partida, o para probar el flujo de cierre asincrónico sin
    /// armar ninguna animación.
    /// </summary>
    public class TimedCloseAnimation : MonoBehaviour, IPopupCloseAnimation
    {
        [SerializeField] float _duration = 0.25f;

        Coroutine _running;

        public void Play(Action onComplete)
        {
            _running = StartCoroutine(Wait(onComplete));
        }

        public void Cancel()
        {
            if (_running != null) StopCoroutine(_running);
            _running = null;
        }

        IEnumerator Wait(Action onComplete)
        {
            yield return new WaitForSeconds(_duration);
            _running = null;
            onComplete();
        }
    }
}
