using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Rendering.Universal;

namespace GameOptions
{
    /// <summary>
    /// Problema: en el Editor, modificar un URP Asset desde código modifica el ARCHIVO del proyecto,
    /// y el cambio sobrevive al salir de Play.
    /// Solucion: recordar los valores originales la primera vez que se toca cada asset
    /// y restaurarlos al salir de Play.
    /// En una build estos metodos ni se compilan en las llamadas ([Conditional]), asi que no cuestan nada.
    /// </summary>
    public sealed class UrpAssetEditorBackup
    {
        private readonly Dictionary<UniversalRenderPipelineAsset, (float shadowDistance, int shadowmapResolution, int msaa)>
            _originals = new Dictionary<UniversalRenderPipelineAsset, (float, int, int)>();

        [Conditional("UNITY_EDITOR")]
        public void Remember(UniversalRenderPipelineAsset asset)
        {
            if (_originals.ContainsKey(asset))
                return; // Solo interesa el valor anterior a nuestro primer cambio

            _originals[asset] = (asset.shadowDistance, asset.mainLightShadowmapResolution, asset.msaaSampleCount);
        }

        [Conditional("UNITY_EDITOR")]
        public void RestoreAll()
        {
            foreach (var pair in _originals)
            {
                UniversalRenderPipelineAsset asset = pair.Key;
                var original = pair.Value;

                if (asset == null)
                    continue;

                asset.shadowDistance = original.shadowDistance;
                asset.mainLightShadowmapResolution = original.shadowmapResolution;
                asset.msaaSampleCount = original.msaa;
            }

            _originals.Clear();
        }
    }
}
