using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GameOptions
{
    /// <summary>
    /// Aplica el nivel de calidad (Project Settings > Quality) y, sobre el URP Asset
    /// que ese nivel activa, las sombras y el MSAA elegidos.
    /// Va en el mismo GameObject que SettingsManager (el URP Asset es global).
    /// El antialiasing por post-proceso (FXAA/SMAA) lo aplica CameraAntialiasingApplier.
    /// </summary>
    public sealed class GraphicsSettingsApplier : SettingsListener
    {
        private readonly UrpAssetEditorBackup _editorBackup = new UrpAssetEditorBackup();

        protected override void Apply(GameSettings settings)
        {
            ApplyQualityLevel(settings.qualityLevel);

            // Cada nivel de calidad puede tener su propio URP Asset: se toma el activo DESPUES de cambiar el nivel.
            if (GraphicsSettings.currentRenderPipeline is not UniversalRenderPipelineAsset urpAsset)
            {
                Debug.LogWarning("GraphicsSettingsApplier: el proyecto no esta usando URP.", this);
                return;
            }

            _editorBackup.Remember(urpAsset);

            urpAsset.shadowDistance = settings.shadows.ToShadowDistance();
            urpAsset.mainLightShadowmapResolution = settings.shadows.ToShadowmapResolution();
            urpAsset.msaaSampleCount = settings.antialiasing.ToMsaaSampleCount();
        }

        private static void ApplyQualityLevel(int level)
        {
            level = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);

            if (QualitySettings.GetQualityLevel() != level)
                QualitySettings.SetQualityLevel(level, applyExpensiveChanges: true);
        }

        private void OnApplicationQuit() => _editorBackup.RestoreAll();
    }
}
