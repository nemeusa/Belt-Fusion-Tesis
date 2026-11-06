namespace GameOptions
{
    public sealed class BrightnessSection : SliderSettingSection
    {
        protected override float MinValue => GameSettings.MinBrightness;
        protected override float MaxValue => GameSettings.MaxBrightness;

        protected override float Read(GameSettings settings) => settings.brightness;
        protected override void Write(GameSettings settings, float value) => settings.brightness = value;

        protected override string FormatValue(float value) => value.ToString("+0.0;-0.0;0.0"); // Ej: +0.5 / -1.0 / 0.0
    }
}
