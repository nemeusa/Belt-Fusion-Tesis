using DG.Tweening;
using UnityEngine;

public class Dinocheckpoint : MonoBehaviour
{
    [SerializeField] Transform jawPivot;
    [SerializeField] Vector3 openRotation = new Vector3(35f, 0f, 0f); // probá el eje que corresponda
    [SerializeField] float openDuration = 0.4f;

    bool _activated;

    void OnTriggerEnter(Collider other)
    {
        if (_activated || !other.GetComponent<PlayerController>()) return;
        _activated = true;

        // acá guardás tu checkpoint, por ejemplo:
        // GameManager.Instance.SetCheckpoint(respawnPoint.position);

        jawPivot.DOLocalRotate(openRotation, openDuration)
                .SetEase(Ease.OutBack);   // un pequeño rebote lo hace sentir más vivo
    }
}
