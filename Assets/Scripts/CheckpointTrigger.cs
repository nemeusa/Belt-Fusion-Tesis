using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool _activated = false;
    [SerializeField] GameObject _meshOb;
    [SerializeField] Transform respawnPost;
    [SerializeField] GameObject symbol;
    [SerializeField] List<ParticleSystem> effects = new List<ParticleSystem>();
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && !_activated)
        {
            symbol.SetActive(true);
            foreach (var p in effects) p.Stop();
            CheckpointManager.Instance.UpdateCheckpoint(respawnPost.position);
            _activated = true;

            _meshOb.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
