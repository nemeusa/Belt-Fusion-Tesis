using UnityEngine;
using UnityEngine.Audio;

namespace GameOptions
{
    /// <summary>Aplica los volúmenes de música y sonido al AudioMixer.</summary>
    public sealed class AudioSettingsApplier : SettingsListener
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private string musicParameter = "MusicVol";
        [SerializeField] private string sfxParameter = "SoundsVol";

        // SetFloat en OnEnable/Awake a veces no llega a aplicarse porque el mixer aún no inicializó:
        // se reaplica en Start para asegurarlo.
        private void Start()
        {
            if (SettingsManager.Instance != null)
                Apply(SettingsManager.Instance.Current);
        }

        protected override void Apply(GameSettings settings)
        {
            mixer.SetFloat(musicParameter, ToDecibels(settings.musicVolume));
            mixer.SetFloat(sfxParameter, ToDecibels(settings.sfxVolume));
        }

        // El oído percibe el volumen de forma logarítmica: un slider lineal 0-1 se convierte a dB.
        // El mínimo evita Log10(0) = -infinito; -80 dB equivale a silencio.
        private static float ToDecibels(float linear) => Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }
}