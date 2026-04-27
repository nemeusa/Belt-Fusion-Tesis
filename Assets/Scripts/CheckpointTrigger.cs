using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool _activated = false;
    [SerializeField] GameObject _meshOb;
    [SerializeField] Transform respawnPost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && !_activated)
        {
            CheckpointManager.Instance.UpdateCheckpoint(respawnPost.position);
            _activated = true;

            _meshOb.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
