using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameOptions
{
    /// <summary>
    /// Aplica el antialiasing por post-proceso (FXAA / SMAA) a esta camara.
    /// Se agrega a cada camara de juego, en cada escena.
    /// Si se eligio MSAA, la camara queda sin post-proceso de AA (MSAA lo maneja GraphicsSettingsApplier).
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed class CameraAntialiasingApplier : SettingsListener
    {
        private UniversalAdditionalCameraData _cameraData;

        private void Awake()
        {
            _cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
        }

        protected override void Apply(GameSettings settings)
        {
            _cameraData.antialiasing = settings.antialiasing.ToCameraAntialiasing();
        }
    }
}
