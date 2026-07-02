using System.Collections;
using UnityEngine;

public class EffectGlitch : MonoBehaviour
{
    [SerializeField] float effectTime = 2;
    [SerializeField] float cooldown = 5;

    [SerializeField] Material glitchMat;

    Material defaultMat;

    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMat = meshRenderer.material;
        StartCoroutine(ActiveEffect());
    }

    IEnumerator ActiveEffect()
    {
        while(true)
        {
           yield return new WaitForSeconds(effectTime);
           meshRenderer.material = defaultMat;
            //Debug.Log("normal mat");
           yield return new WaitForSeconds(cooldown);
           meshRenderer.material = glitchMat;
            //Debug.Log("glitch mat");

        }
    }
}
