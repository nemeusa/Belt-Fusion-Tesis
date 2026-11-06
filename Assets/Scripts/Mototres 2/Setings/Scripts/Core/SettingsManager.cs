using System;
using UnityEngine;

namespace GameOptions
{
    /// <summary>
    /// Duenio de la configuracion aplicada. Su unica responsabilidad es guardarla y
    /// avisar cuando cambia: no sabe nada de pantallas, URP, UI ni jugadores.
    /// Quien necesite reaccionar a la configuracion se suscribe a <see cref="SettingsApplied"/>.
    ///
    /// Regla de uso: los GameSettings que entrega este manager son de SOLO LECTURA.
    /// Para cambiar algo: Current.Clone() -> modificar la copia -> Apply(copia).
    /// </summary>
    [DefaultExecutionOrder(-1000)] // Se inicializa antes que cualquier script que lo use
    public sealed class SettingsManager : MonoBehaviour
    {
        /// <summary>
        /// Acceso global. Es una concesión práctica: el manager vive entre escenas
        /// y así cualquier script lo encuentra sin cablear referencias a mano.
        /// </summary>
        public static SettingsManager Instance { get; private set; }

        public GameSettings Current { get; private set; }

        public event Action<GameSettings> SettingsApplied;

        private ISettingsStorage _storage;

        private void Awake()
        {
            // Permite tener un SettingsManager en cada escena para probarlas por separado:
            // si ya existe uno de una escena anterior, este se descarta.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            TryGetComponent(out _storage);
            Current = _storage?.Load() ?? GameSettings.CreateDefault();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        /// <summary>Aplica una nueva configuración, la guarda y notifica a todos los interesados.</summary>
        public void Apply(GameSettings settings)
        {
            Current = settings.Clone(); // Copia propia: cambios posteriores del llamador no la afectan
            _storage?.Save(Current);
            SettingsApplied?.Invoke(Current);
        }
    }
}
