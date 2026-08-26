using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevels : MonoBehaviour
{
    [Header("Botones de UI")]
    [SerializeField] private Button botonNivel1;
    [SerializeField] private Button botonNivel2;

    [Header("Opcional: Visuales")]
    [SerializeField] private GameObject iconoCandadoNivel2; // Icono visual de candado (opcional)

    [SerializeField] TMP_Text bestTimeLevelOne;
    [SerializeField] TMP_Text bestTimeLevelTwo;

    private void OnEnable()
    {
        ActualizarEstadoBotones();

    }

    private void Start()
    {
        BestTimeLevels();
        
    }
    public void BestTimeLevels()
    {
       
        if (PlayerPrefs.HasKey("BestTimeLevel1"))
        bestTimeLevelOne.text = $"BEST RECORD: {CreateCounter(PlayerPrefs.GetFloat($"BestTimeLevel1"))}";
        if (PlayerPrefs.HasKey("BestTimeLevel2"))
        bestTimeLevelTwo.text = $"BEST RECORD: {CreateCounter(PlayerPrefs.GetFloat($"BestTimeLevel2"))}";

        //PlayerPrefs.GetString($"BestTime{currentLevel}", GameManager.instance.counterGame);
    }

    string CreateCounter(float seconds)
    {
        int minutos = Mathf.FloorToInt(seconds / 60);
        int segs = Mathf.FloorToInt(seconds % 60);

        // Multiplicamos el resto decimal por 100 para obtener dos dígitos de milisegundos
        int milisegundos = Mathf.FloorToInt((seconds % 1) * 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutos, segs, milisegundos);

    }

    public void DeleteData()
    {
            PlayerPrefs.DeleteAll();
            Debug.Log("¡Progreso borrado para testing!");
            ActualizarEstadoBotones();
    }

    public void ActualizarEstadoBotones()
    {
        // El Nivel 1 siempre está disponible
        botonNivel1.interactable = true;

        // Leemos si el Nivel 2 está desbloqueado.
        // El segundo parámetro (0) es el valor por defecto si la clave todavía no existe.
        bool nivel2EstaDesbloqueado = PlayerPrefs.GetInt("Nivel2Desbloqueado", 0) == 1;

        // Si interactable es false, el botón de Unity UI no responde a clicks ni a joysticks
        botonNivel2.interactable = nivel2EstaDesbloqueado;

        // Si pusiste un dibujito de candado encima del botón, lo apagás cuando se desbloquea
        if (iconoCandadoNivel2 != null)
        {
            iconoCandadoNivel2.SetActive(!nivel2EstaDesbloqueado);
        }
    }
}
