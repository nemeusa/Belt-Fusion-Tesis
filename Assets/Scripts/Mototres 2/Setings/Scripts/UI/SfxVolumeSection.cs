using UnityEngine;


namespace GameOptions
{
    public sealed class SfxVolumeSection : SliderSettingSection
    {
        protected override float MinValue => GameSettings.MinVolume;
        protected override float MaxValue => GameSettings.MaxVolume;

        protected override float Read(GameSettings settings) => settings.sfxVolume;
        protected override void Write(GameSettings settings, float value) => settings.sfxVolume = value;

        protected override string FormatValue(float value) => $"{Mathf.RoundToInt(value * 100)}%";
    }
}