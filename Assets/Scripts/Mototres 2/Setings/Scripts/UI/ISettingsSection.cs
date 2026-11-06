namespace GameOptions
{
    /// <summary>
    /// Una parte del menu (Pantalla, Graficos, Brillo...). Cada seccion conoce solo sus controles
    /// y los campos de GameSettings que le corresponden.
    /// OptionsMenu encuentra todas las secciones automaticamente: agregar una nueva no requiere tocarlo.
    /// </summary>
    public interface ISettingsSection
    {
        /// <summary>Muestra en los controles los valores de <paramref name="settings"/>.</summary>
        void Load(GameSettings settings);

        /// <summary>Vuelca lo elegido en los controles dentro de <paramref name="settings"/>.</summary>
        void WriteTo(GameSettings settings);
    }
}
