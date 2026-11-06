using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameOptions
{
    /// <summary>
    /// Base para opciones de un solo valor numerico (slider + texto con el valor).
    /// Las clases hijas solo indican el rango y qué campo de GameSettings leen y escriben.
    /// </summary>
    public abstract class SliderSettingSection : MonoBehaviour, ISettingsSection
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text valueLabel; // Opcional

        protected abstract float MinValue { get; }
        protected abstract float MaxValue { get; }
        protected abstract float Read(GameSettings settings);
        protected abstract void Write(GameSettings settings, float value);

        protected virtual string FormatValue(float value) => value.ToString("0.00");

        private void Awake()
        {
            slider.minValue = MinValue;
            slider.maxValue = MaxValue;
            slider.onValueChanged.AddListener(UpdateLabel);
        }

        public void Load(GameSettings settings)
        {
            float value = Read(settings);
            slider.SetValueWithoutNotify(value); // Sin notificar: el texto se actualiza a mano
            UpdateLabel(slider.value);           // slider.value ya viene limitado al rango
        }

        public void WriteTo(GameSettings settings) => Write(settings, slider.value);

        private void UpdateLabel(float value)
        {
            if (valueLabel != null)
                valueLabel.text = FormatValue(value);
        }
    }
}
