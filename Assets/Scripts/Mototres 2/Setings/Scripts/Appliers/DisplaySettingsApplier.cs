using UnityEngine;

namespace GameOptions
{
    /// <summary>
    /// Aplica resolucion, frecuencia y modo de pantalla.
    /// Va en el mismo GameObject que SettingsManager (la pantalla es global, no depende de la escena).
    /// Nota: en el Editor Screen.SetResolution no tiene efecto visible, habria que probarlo en una build.
    /// </summary>
    public sealed class DisplaySettingsApplier : SettingsListener
    {
        protected override void Apply(GameSettings settings)
        {
            if (settings.resolutionWidth <= 0 || settings.resolutionHeight <= 0)
                return; // Datos invalidos (ej. un archivo guardado corrupto)

            RefreshRate refreshRate = settings.GetRefreshRate();

            if (IsAlreadyApplied(settings, refreshRate))
                return; // Evita el parpadeo de reaplicar la misma resolucion

            Screen.SetResolution(settings.resolutionWidth, settings.resolutionHeight,
                                 settings.fullScreenMode, refreshRate);
        }

        private static bool IsAlreadyApplied(GameSettings settings, RefreshRate refreshRate)
        {
            return Screen.width == settings.resolutionWidth
                && Screen.height == settings.resolutionHeight
                && Screen.fullScreenMode == settings.fullScreenMode
                && Mathf.Approximately((float)Screen.currentResolution.refreshRateRatio.value,
                                       (float)refreshRate.value);
        }
    }
}
