using UnityEngine;

namespace GameOptions
{
    public sealed class MusicVolumeSection : SliderSettingSection
    {
        protected override float MinValue => GameSettings.MinVolume;
        protected override float MaxValue => GameSettings.MaxVolume;

        protected override float Read(GameSettings settings) => settings.musicVolume;
        protected override void Write(GameSettings settings, float value) => settings.musicVolume = value;

        protected override string FormatValue(float value) => $"{Mathf.RoundToInt(value * 100)}%";
    }
}