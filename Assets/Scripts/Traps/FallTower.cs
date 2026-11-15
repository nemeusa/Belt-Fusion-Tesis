using DG.Tweening;
using UnityEngine;

public class FallTower : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform pivoteTorre;       // el vacío en la base de la torre

    [Header("Configuración")]
    [SerializeField] private bool caeHaciaIzquierda = true;
    [SerializeField] private float anguloInclinacion = 65f;
    [SerializeField] private float duracion = 2f;
    [SerializeField] private Ease ease = Ease.InQuad;

    private bool yaCayo;

    private void OnTriggerEnter(Collider other)
    {
        if (yaCayo || other.TryGetComponent<PlayerController>(out var player)) return;

        yaCayo = true;

        float lado = caeHaciaIzquierda ? 1f : -1f;
        Vector3 destino = pivoteTorre.localEulerAngles + new Vector3(0f, 0f, anguloInclinacion * lado);

        pivoteTorre
            .DOLocalRotate(destino, duracion, RotateMode.Fast)
            .SetEase(ease);
    }
}
