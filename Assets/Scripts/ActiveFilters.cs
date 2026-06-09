using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ActiveFilters : MonoBehaviour
{
    [Header("Configuración del Filtro")]
    public string nombreDelFiltro = "BlancoYNegro";

    private ScriptableRendererFeature _filtroBlancoNegro;

    void Start()
    {
        var pipelineAsset = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

        if (pipelineAsset == null)
        {
            Debug.LogError("No se encontró el UniversalRenderPipelineAsset activo.");
            return;
        }

        var propertyInfo = typeof(UniversalRenderPipelineAsset).GetProperty("scriptableRendererData",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (propertyInfo != null)
        {
            var rendererData = propertyInfo.GetValue(pipelineAsset) as ScriptableRendererData;

            if (rendererData != null)
            {
                // 3. Buscamos nuestro filtro por su nombre en la lista del renderer
                foreach (var feature in rendererData.rendererFeatures)
                {
                    if (feature.name == nombreDelFiltro)
                    {
                        _filtroBlancoNegro = feature;
                        break;
                    }
                }
            }
        }

        if (_filtroBlancoNegro == null)
        {
        }
    }

    public void AlternarFiltroBlancoNegro(bool activar)
    {
        if (_filtroBlancoNegro != null)
        {
            _filtroBlancoNegro.SetActive(activar);
        }
    }
}
