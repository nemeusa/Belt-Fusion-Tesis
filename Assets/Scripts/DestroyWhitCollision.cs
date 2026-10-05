using UnityEngine;

public class DestroyWhitCollision : MonoBehaviour
{
    [SerializeField] float destroyTimeAfterCollWhitPlayer;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] bool callDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (callDestroy) return;
        if (other.gameObject.layer == ignoreLayer) return;
        
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        Destroy(gameObject, destroyTimeAfterCollWhitPlayer);

        //else Destroy(gameObject);
    }

    public void ActiveDestroy(Collider target)
    {
        if (!callDestroy) return;
        if (target.gameObject.layer == ignoreLayer) return;

        Destroy(gameObject);

    }
}
