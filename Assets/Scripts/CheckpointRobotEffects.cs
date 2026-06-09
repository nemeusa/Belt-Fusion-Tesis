using System.Collections;
using UnityEngine;

public class CheckpointRobotEffects : MonoBehaviour
{
    [Header("Configuración del Mesh")]
    [SerializeField] private MeshRenderer meshRobot;

    [Header("Configuración del Efecto")]
    [SerializeField] private string nombreVariableShader = "_Alpha";
    [SerializeField] private float velocidadTransicion = 2f;

    private Material materialBrillo;
    private Coroutine corrutinaActual;

    // Tus valores invertidos
    private readonly float alphaApagado = 1.81f;
    private readonly float alphaMaximoBrillo = 0f;

    void Start()
    {
        if (meshRobot == null)
            meshRobot = GetComponentInChildren<MeshRenderer>();

        if (meshRobot != null && meshRobot.materials.Length > 1)
        {
            materialBrillo = meshRobot.materials[1];
            // Nos aseguramos de que arranque en su estado estándar apagado
            materialBrillo.SetFloat(nombreVariableShader, alphaApagado);
        }
    }

    // 1. LLAMAR ESTO DESDE EL ONTRIGGERENTER DEL CHECKPOINT
    public void EncenderBrilloCheckpoint()
    {
        if (materialBrillo == null) return;

        if (corrutinaActual != null) StopCoroutine(corrutinaActual);
        corrutinaActual = StartCoroutine(TransicionBrillo(alphaMaximoBrillo, velocidadTransicion * 3f));
    }

    // 2. LLAMAR ESTO DESDE EL ONTRIGGEREXIT DEL CHECKPOINT
    public void ApagarBrilloCheckpoint()
    {
        if (materialBrillo == null) return;

        if (corrutinaActual != null) StopCoroutine(corrutinaActual);
        corrutinaActual = StartCoroutine(TransicionBrillo(alphaApagado, velocidadTransicion));
    }

    // Una corrutina genérica que sirve para ir hacia cualquier valor de Alpha
    private IEnumerator TransicionBrillo(float objetivoAlpha, float velocidad)
    {
        float alphaActual = materialBrillo.GetFloat(nombreVariableShader);

        // Si el objetivo es 0 (brillar), vamos restando. Si es 1.81 (apagar), vamos sumando.
        if (objetivoAlpha == alphaMaximoBrillo)
        {
            while (alphaActual > alphaMaximoBrillo)
            {
                alphaActual -= Time.deltaTime * velocidad;
                materialBrillo.SetFloat(nombreVariableShader, Mathf.Max(alphaActual, alphaMaximoBrillo));
                yield return null;
            }
        }
        else
        {
            while (alphaActual < alphaApagado)
            {
                alphaActual += Time.deltaTime * velocidad;
                materialBrillo.SetFloat(nombreVariableShader, Mathf.Min(alphaActual, alphaApagado));
                yield return null;
            }
        }
    }
}
