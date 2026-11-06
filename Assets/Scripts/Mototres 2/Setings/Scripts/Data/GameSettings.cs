using NUnit.Framework;
using System;
using UnityEditor;
using UnityEngine;

namespace GameOptions
{
    /// <summary>
    /// Configuracion elegida por el jugador. Es solo datos.
    /// Es el objeto que se guarda y se carga desde disco.
    /// </summary>
    [Serializable]
    public class GameSettings
    {
        // Rangos validos. Los usan tanto la UI (limites de los sliders) como quien quiera validar.
        public const float MinBrightness = -2f;
        public const float MaxBrightness = 2f;
        public const float MinMouseSensitivity = 0.1f;
        public const float MaxMouseSensitivity = 5f;

        // Screen
        public int resolutionWidth;
        public int resolutionHeight;
        // La frecuencia se guarda como fraccion, igual como la expone Unity (ej. 59.94 Hz = 60000 / 1001).
        public uint refreshRateNumerator;
        public uint refreshRateDenominator;
        public FullScreenMode fullScreenMode;

        // Graficos
        public int qualityLevel;               // Indice de Project Settings > Quality
        public ShadowSetting shadows;
        public AntialiasingOption antialiasing;

        // Iluminacion
        // Exposicion en EV ("stops"): 0 = sin cambio, +1 = el doble de luz, -1 = la mitad.
        public float brightness;

        // Constantes(junto a las otras)
        public const float MinVolume = 0f;
        public const float MaxVolume = 1f;

        // Audio
        // Con inicializador: si ya tienes un archivo guardado sin estos campos, cargan en 1 y no en 0 (silencio).
        public float musicVolume = 1f;
        public float sfxVolume = 1f;

        // Controles
        public float mouseSensitivity;

        /// <summary>Valores iniciales cuando no hay nada guardado.</summary>
        public static GameSettings CreateDefault()
        {
            var settings = new GameSettings
            {
                fullScreenMode = FullScreenMode.FullScreenWindow,
                qualityLevel = QualitySettings.GetQualityLevel(),
                shadows = ShadowSetting.High,
                antialiasing = AntialiasingOption.FXAA,
                brightness = 0f,
                mouseSensitivity = 1f
            };
            settings.SetResolution(Screen.currentResolution); // Resolucion nativa del monitor
            return settings;
        }


        public GameSettings Clone() => (GameSettings)MemberwiseClone();

        public void SetResolution(Resolution resolution)
        {
            resolutionWidth = resolution.width;
            resolutionHeight = resolution.height;
            refreshRateNumerator = resolution.refreshRateRatio.numerator;
            refreshRateDenominator = resolution.refreshRateRatio.denominator;
        }

        public RefreshRate GetRefreshRate() => new RefreshRate
        {
            numerator = refreshRateNumerator,
            denominator = Math.Max(1u, refreshRateDenominator) // Evita dividir por cero con datos invalidos
        };
    }
}
