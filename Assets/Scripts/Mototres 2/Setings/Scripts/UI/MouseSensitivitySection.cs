namespace GameOptions
{
    /// <summary>Seccion "Sensibilidad del mouse": multiplicador (1 = valor base del juego).</summary>
    public sealed class MouseSensitivitySection : SliderSettingSection
    {
        protected override float MinValue => GameSettings.MinMouseSensitivity;
        protected override float MaxValue => GameSettings.MaxMouseSensitivity;

        protected override float Read(GameSettings settings) => settings.mouseSensitivity;
        protected override void Write(GameSettings settings, float value) => settings.mouseSensitivity = value;

        protected override string FormatValue(float value) => $"{value:0.00}x";
    }
}
