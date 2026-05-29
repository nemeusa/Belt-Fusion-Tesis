using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CheckpointTrigger : MonoBehaviour
{
    private bool _activated = false;
    [SerializeField] GameObject _meshOb;
    [SerializeField] Transform respawnPost;
    //[SerializeField] GameObject meshPlatillo;
    [SerializeField] Material paloCheckpointMat;
    [SerializeField] Material baseCheckpointMat;
    [SerializeField] MeshRenderer[] palosCheckpointMesh;
    [SerializeField] MeshRenderer baseCheckpointMesh;

    [SerializeField] List<VisualEffect> effects = new List<VisualEffect>();

    [SerializeField] Transform robotPosition;

    private void Awake()
    {
        //meshPlatillo.SetActive(false);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player) && !_activated)
        {
            player.robot.CheckpointAction(robotPosition, this);
         
        }
    }

    public void CreateCheckpoint()
    {
        //meshPlatillo.SetActive(true);
        //foreach (var p in effects) p.SendEvent("OnPlay");
        baseCheckpointMesh.material = baseCheckpointMat;
        foreach (var p in palosCheckpointMesh) p.material = paloCheckpointMat;

        CheckpointManager.Instance.UpdateCheckpoint(respawnPost.position);
        _activated = true;

        _meshOb.GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
