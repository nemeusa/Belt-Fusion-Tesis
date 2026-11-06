using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameOptions
{
    /// <summary>Seccion "Screen": resolucion (con frecuencia) y modo de pantalla.</summary>
    public sealed class DisplaySection : MonoBehaviour, ISettingsSection
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown fullScreenDropdown;

        // Ambos arrays van en el mismo orden: el indice del dropdown elige el modo.
        private static readonly FullScreenMode[] FullScreenModes =
        {
            FullScreenMode.FullScreenWindow,
            FullScreenMode.Windowed
        };

        private static readonly string[] FullScreenLabels =
        {
            "Pantalla completa",
            "Ventana"
        };

        private Resolution[] _resolutions;

        private void Awake()
        {
            // Screen.resolutions depende del monitor y puede traer entradas repetidas:
            // se descartan duplicados y se ordena de mayor a menor.
            _resolutions = Screen.resolutions
                .GroupBy(r => (r.width, r.height, RoundedHz(r)))
                .Select(group => group.First())
                .OrderByDescending(r => r.width * r.height)
                .ThenByDescending(RoundedHz)
                .ToArray();

            resolutionDropdown.SetOptions(_resolutions.Select(r => $"{r.width} x {r.height} @ {RoundedHz(r)} Hz"));
            fullScreenDropdown.SetOptions(FullScreenLabels);
        }

        public void Load(GameSettings settings)
        {
            resolutionDropdown.SelectWithoutNotify(FindClosestResolutionIndex(settings));
            fullScreenDropdown.SelectWithoutNotify(Array.IndexOf(FullScreenModes, settings.fullScreenMode));
        }

        public void WriteTo(GameSettings settings)
        {
            if (_resolutions.Length > 0)
                settings.SetResolution(_resolutions[resolutionDropdown.value]);

            settings.fullScreenMode = FullScreenModes[fullScreenDropdown.value];
        }

        /// <summary>
        /// Lo guardado puede no existir en este monitor (Ej. el juego se abrio en otra PC),
        /// por eso se busca la resolución más parecida en lugar de exigir una coincidencia exacta.
        /// </summary>
        private int FindClosestResolutionIndex(GameSettings settings)
        {
            int savedHz = (int)Math.Round(settings.GetRefreshRate().value);
            int bestIndex = 0;
            int bestDifference = int.MaxValue;

            for (int i = 0; i < _resolutions.Length; i++)
            {
                Resolution r = _resolutions[i];
                int difference = Math.Abs(r.width - settings.resolutionWidth)
                               + Math.Abs(r.height - settings.resolutionHeight)
                               + Math.Abs(RoundedHz(r) - savedHz);

                if (difference < bestDifference)
                {
                    bestDifference = difference;
                    bestIndex = i;
                }
            }

            return bestIndex;
        }

        private static int RoundedHz(Resolution resolution) => (int)Math.Round(resolution.refreshRateRatio.value);
    }
}
