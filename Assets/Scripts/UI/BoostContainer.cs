using TMPro;
using UnityEngine;

public class BoostContainer : MonoBehaviour
{
    [SerializeField] BoostUI[] _boosts;

    [SerializeField] GameObject _defaultSimbol;
    [SerializeField] GameObject _fireSimbol;
    [SerializeField] GameObject _energySimbol;
    [SerializeField] GameObject _iceSimbol;

    [SerializeField] GameObject _offDefaultSimbol;
    [SerializeField] GameObject _offFireSimbol;
    [SerializeField] GameObject _offEnergySimbol;
    [SerializeField] GameObject _offIceSimbol;
    [SerializeField, Range(0.0f, 0.1f)] float diferentSizeTextCounter = 0.05f;
    [SerializeField, Range(1.0f, 4f)] float spaceCharacterTextCounter = 1.5f;


    private float _fountSizeDefault;

    public float seconds;
    [SerializeField] TMP_Text counterText;

    string counter;

    TypeFSM oldElement = TypeFSM.Default;

    private void Awake()
    {
        oldElement = TypeFSM.Default;
        _fountSizeDefault = counterText.fontSize;
    }

    private void Update()
    {
        TimerCount();
    }

    void TimerCount()
    {
        if (GameManager.instance.winGame)
        {
            GameManager.instance.counterGame = seconds;
            return;

        }

        seconds += Time.deltaTime;

        int minutos = Mathf.FloorToInt(seconds / 60);
        int segs = Mathf.FloorToInt(seconds % 60);

        // Multiplicamos el resto decimal por 100 para obtener dos dígitos de milisegundos
        int milisegundos = Mathf.FloorToInt((seconds % 1) * 100);

        counter = string.Format("{0:00}:{1:00}:{2:00}", minutos, segs, milisegundos);

        // Agregamos el tercer campo {2:00} al formato del string
        counterText.text = counter;

        //seconds += Time.deltaTime;

        //int minutos = Mathf.FloorToInt(seconds / 60);
        //int segs = Mathf.FloorToInt(seconds % 60);

        //counterText.text = string.Format("{0:00}:{1:00}", minutos, segs);

        //// Modificar el ancho del RectTransform
        //RectTransform rect = counterText.rectTransform;
        //rect.pivot = new Vector2(0f, rect.pivot.y); // Fija el pivote a la izquierda
        //rect.sizeDelta = new Vector2(100 + counter.Length, rect.sizeDelta.y);

        //// Construimos el texto envolviendo cada carácter en su propia etiqueta <size>
        //StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < counter.Length; i++)
        //{
        //    float tamañoActual = _fountSizeDefault + (i * 2);
        //    sb.Append($"<size={tamañoActual}>{counter[i]}</size>");
        //}

        //counterText.alignment = TMPro.TextAlignmentOptions.Left; // Asegura crecimiento a la derecha
        //counterText.text = sb.ToString();

        //// Asegurar alineación a la izquierda para que crezca hacia la derecha
        //counterText.alignment = TMPro.TextAlignmentOptions.Left;

        //// Sumar 1 punto de tamaño de fuente por cada carácter
        //counterText.fontSize = _fountSizeDefault + counter.Length;


        // 1. Forzar a TextMeshPro a calcular el texto base
        counterText.ForceMeshUpdate();

        TMP_TextInfo textInfo = counterText.textInfo;
        int characterCount = textInfo.characterCount;

        float desplazamientoXTotal = 0f;

        for (int i = 0; i < characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            // Ignorar si el carácter no es visible (como espacios), pero acumular espaciado si es necesario
            if (!charInfo.isVisible)
            {
                // Si es un espacio, añadimos el espacio acumulado correspondiente a su posición
                desplazamientoXTotal += (i * spaceCharacterTextCounter);
                continue;
            }

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // --- PASO A: CALCULAR EL CENTRO BASE PARA ESCALAR ---
            Vector3 charBottomLeft = vertices[vertexIndex + 0];
            Vector3 charTopRight = vertices[vertexIndex + 2];
            Vector3 center = (charBottomLeft + charTopRight) * 0.5f;
            Vector3 basePoint = new Vector3(center.x, charBottomLeft.y, center.z);

            // Escala progresiva de tamaño (Letra 0 = x1, Letra 1 = x1.2, Letra 2 = x1.4...)
            float escala = 1.0f + (i * diferentSizeTextCounter);

            // --- PASO B: CALCULAR EL DESPLAZAMIENTO DE ESPACIADO ---
            // Letra 0 se mueve 0, Letra 1 se mueve 1*espaciado, Letra 2 se mueve (1+2)*espaciado, etc.
            desplazamientoXTotal += (i * spaceCharacterTextCounter);

            // --- PASO C: APLICAR TAMAÑO Y LUEGO DESPLAZAR VÉRTICES ---
            for (int j = 0; j < 4; j++)
            {
                Vector3 origVertex = vertices[vertexIndex + j];

                // 1. Escalamos la letra respecto a su propia base
                Vector3 verticeEscalado = basePoint + (origVertex - basePoint) * escala;

                // 2. Desplazamos toda la letra hacia la derecha en el eje X
                verticeEscalado.x += desplazamientoXTotal;

                // Guardamos el resultado final en el array de vértices
                vertices[vertexIndex + j] = verticeEscalado;
            }
        }

        // 2. Renderizar y actualizar los cambios en la pantalla
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            counterText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
    //counterText.ForceMeshUpdate();

    //TMP_TextInfo textInfo = counterText.textInfo;
    //int characterCount = textInfo.characterCount;

    //for (int i = 0; i < characterCount; i++)
    //{
    //    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

    //    // Ignorar si el carácter no es visible (como los espacios)
    //    if (!charInfo.isVisible) continue;

    //    // Obtener el índice del material y de los vértices
    //    int materialIndex = charInfo.materialReferenceIndex;
    //    int vertexIndex = charInfo.vertexIndex;

    //    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

    //    // Calcular el centro inferior de la letra para que escale desde la base
    //    Vector3 charBottomLeft = vertices[vertexIndex + 0];
    //    Vector3 charTopRight = vertices[vertexIndex + 2];
    //    Vector3 center = (charBottomLeft + charTopRight) * 0.5f;
    //    Vector3 basePoint = new Vector3(center.x, charBottomLeft.y, center.z);

    //    // DEFINIR LA ESCALA: Aquí decides el tamaño de cada letra.
    //    // En este ejemplo, el tamaño aumenta según la posición 'i' de la letra.
    //    float escala = 1.0f + (i * diferentSizeTextCounter);
    //    //float escala = 1.2f;

    //    // Aplicar la escala a los 4 vértices que componen la letra
    //    for (int j = 0; j < 4; j++)
    //    {
    //        Vector3 origVertex = vertices[vertexIndex + j];
    //        // Escalar respecto al punto base de la letra
    //        vertices[vertexIndex + j] = basePoint + (origVertex - basePoint) * escala;
    //    }
    //}

    //// Notificar a TextMeshPro que actualice la geometría del renderizado
    //for (int i = 0; i < textInfo.meshInfo.Length; i++)
    //{
    //    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
    //    counterText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
    //}

    public void BoostsActive(int actualBoost)
    {
        for (int i = 0; i < _boosts.Length; i++)
        {
            if (i < actualBoost) _boosts[i].ActiveBoost();

            else _boosts[i].DesactiveBoost();
        }
    }

    public void ChangeSymbol(TypeFSM newElement)
    {

        ActivateSymbol(newElement).SetActive(true);
        DesactivateSymbol(newElement).SetActive(false);

        if (newElement == oldElement) return;
        ActivateSymbol(oldElement).SetActive(false);
        DesactivateSymbol(oldElement).SetActive(true);

        oldElement = newElement;
    }

    GameObject ActivateSymbol(TypeFSM newElement)
    {
        switch (newElement)
        {
            case TypeFSM.Fire :
            return _fireSimbol;

            case TypeFSM.Electricity:
                return _energySimbol;

            case TypeFSM.Ice:
                return _iceSimbol;

            default:
                return _defaultSimbol;
        }

    }

    GameObject DesactivateSymbol(TypeFSM newElement)
    {
        switch (newElement)
        {
            case TypeFSM.Fire:
                return _offFireSimbol;

            case TypeFSM.Electricity:
                return _offEnergySimbol;

            case TypeFSM.Ice:
                return _offIceSimbol;

            default:
                return _offDefaultSimbol;
        }

    }
}
