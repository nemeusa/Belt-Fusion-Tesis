using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using NUnit.Framework;

public class VolController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private AudioMixer myAudioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string MasterParameter = "MusicVol"; // Nombre del parámetro expuesto en el AudioMixer


    [System.Serializable]
    public class VolLevels
    {
        public string volID;
        [SerializeField] private Slider volumeSlider;

    }

    [SerializeField] List<VolLevels> volsList = new List<VolLevels>();

    private void Start()
    {
        // Asegurarse de que el slider refleje el volumen guardado o por defecto
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
        else
        {
            SetVolume(volumeSlider.value);
        }

        // Escuchar cuando el valor del slider cambie
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        // Convertimos el valor lineal del slider (0.0001 a 1) a decibelios (dB)
        // Usamos Math.Log10 para que la escala del volumen se sienta natural al oído humano.
        float deafibels = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        myAudioMixer.SetFloat(MasterParameter, deafibels);

        // Guardar la preferencia del usuario
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        PlayerPrefs.Save();
    }
}
