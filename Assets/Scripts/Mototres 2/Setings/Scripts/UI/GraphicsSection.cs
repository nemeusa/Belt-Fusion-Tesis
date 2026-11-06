using TMPro;
using UnityEngine;

namespace GameOptions
{
    /// <summary>
    /// Seccion "Graficos": nivel de calidad, sombras y antialiasing.
    /// El nivel de calidad elige el URP Asset base (Project Settings > Quality),
    /// sombras y antialiasing se aplican por encima de ese preset.
    /// </summary>
    public sealed class GraphicsSection : MonoBehaviour, ISettingsSection
    {
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private TMP_Dropdown shadowsDropdown;
        [SerializeField] private TMP_Dropdown antialiasingDropdown;

        // Mismo orden que los enums ShadowSetting y AntialiasingOption: el indice del dropdown es el valor del enum.
        private static readonly string[] ShadowLabels = { "Desactivadas", "Bajas", "Medias", "Altas" };
        private static readonly string[] AntialiasingLabels = { "Desactivado", "FXAA", "SMAA", "MSAA 2x", "MSAA 4x", "MSAA 8x" };

        private void Awake()
        {
            qualityDropdown.SetOptions(QualitySettings.names); // Los nombres definidos en Project Settings > Quality
            shadowsDropdown.SetOptions(ShadowLabels);
            antialiasingDropdown.SetOptions(AntialiasingLabels);
        }

        public void Load(GameSettings settings)
        {
            qualityDropdown.SelectWithoutNotify(settings.qualityLevel);
            shadowsDropdown.SelectWithoutNotify((int)settings.shadows);
            antialiasingDropdown.SelectWithoutNotify((int)settings.antialiasing);
        }

        public void WriteTo(GameSettings settings)
        {
            settings.qualityLevel = qualityDropdown.value;
            settings.shadows = (ShadowSetting)shadowsDropdown.value;
            settings.antialiasing = (AntialiasingOption)antialiasingDropdown.value;
        }
    }
}
