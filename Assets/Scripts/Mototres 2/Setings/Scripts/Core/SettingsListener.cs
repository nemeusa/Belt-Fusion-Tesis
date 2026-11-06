using UnityEngine;

namespace GameOptions
{
    /// <summary>
    /// Base para los componentes que aplican una parte de la configuracion al motor.
    /// Al habilitarse aplica la configuracion actual y queda escuchando cambios.
    /// Gracias a esto, una camara o un Volume de una escena nueva quedan configurados
    /// apenas se cargan, sin que el SettingsManager tenga que conocerlos.
    /// </summary>
    public abstract class SettingsListener : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            var manager = SettingsManager.Instance;
            if (manager == null)
            {
                Debug.LogWarning($"{GetType().Name}: no hay un SettingsManager en la escena.", this);
                return;
            }

            manager.SettingsApplied += Apply;
            Apply(manager.Current);
        }

        protected virtual void OnDisable()
        {
            if (SettingsManager.Instance != null)
                SettingsManager.Instance.SettingsApplied -= Apply;
        }

        /// <param name="settings">Solo lectura: no modificar.</param>
        protected abstract void Apply(GameSettings settings);
    }
}
