namespace GameOptions
{
    /// <summary>
    /// Punto de extension para la persistencia. Cada proyecto lo implementa como quiera
    /// (JSON en disco, PlayerPrefs, binario, nube...) en un MonoBehaviour ubicado
    /// en el mismo GameObject que SettingsManager.
    /// Si no hay ninguna implementacion, el sistema funciona igual, pero sin guardar.
    /// </summary>
    public interface ISettingsStorage
    {
        /// <summary>Devuelve la configuración guardada, o null si todavía no hay nada guardado.</summary>
        GameSettings Load();

        void Save(GameSettings settings);
    }
}
