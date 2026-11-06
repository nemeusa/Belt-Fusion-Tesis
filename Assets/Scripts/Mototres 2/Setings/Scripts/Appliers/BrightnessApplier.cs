using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GameOptions
{
    /// <summary>
    /// Aplica el brillo como exposición (Color Adjustments > Post Exposure) en un Volume global.
    /// En PC Unity no tiene un "brillo de pantalla" nativo, por eso se resuelve con post-procesado.
    /// Requisito: la camara debe tener activado Rendering > Post Processing.
    /// </summary>
    [RequireComponent(typeof(Volume))]
    public sealed class BrightnessApplier : SettingsListener
    {
        private ColorAdjustments _colorAdjustments;

        private void Awake()
        {
            // volume.profile (no sharedProfile) crea una copia en memoria:
            // asi no se modifica el asset del perfil guardado en el proyecto.
            VolumeProfile profile = GetComponent<Volume>().profile;

            if (!profile.TryGet(out _colorAdjustments))
                _colorAdjustments = profile.Add<ColorAdjustments>(overrides: false);
        }

        protected override void Apply(GameSettings settings)
        {
            _colorAdjustments.postExposure.Override(settings.brightness);
        }
    }
}
