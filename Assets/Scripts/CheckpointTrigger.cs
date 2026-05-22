using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.VFX;

public class CheckpointTrigger : MonoBehaviour
{
    private bool _activated = false;
    [SerializeField] GameObject _meshOb;
    [SerializeField] Transform respawnPost;
    [SerializeField] GameObject symbol;
    [SerializeField] List<VisualEffect> effects = new List<VisualEffect>();

    [SerializeField] Transform robotPosition;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player) && !_activated)
        {
            player.robot.CheckpointAction(robotPosition);
            symbol.SetActive(true);
            foreach (var p in effects) p.SendEvent("OnPlay");
            CheckpointManager.Instance.UpdateCheckpoint(respawnPost.position);
            _activated = true;

            _meshOb.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
