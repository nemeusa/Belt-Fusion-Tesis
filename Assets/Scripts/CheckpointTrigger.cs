using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CheckpointTrigger : MonoBehaviour
{
    public bool activated = false;
    [SerializeField] Transform respawnPost;
    //[SerializeField] GameObject meshPlatillo;
    [SerializeField] Material paloCheckpointMat;
    [SerializeField] Material baseCheckpointMat;
    [SerializeField] MeshRenderer[] palosCheckpointMesh;
    [SerializeField] MeshRenderer baseCheckpointMesh;

    [SerializeField] List<VisualEffect> effects = new List<VisualEffect>();

    [SerializeField] Transform robotPosition;

    [SerializeField] Animator aniController;

    private void Awake()
    {
        //meshPlatillo.SetActive(false);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            foreach (var p in effects) p.SendEvent("OnPlay");

            player.robot.goCheckPoint = true;
            player.robot.CheckpointAction(robotPosition, this);
         
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            foreach (var p in effects) p.SendEvent("OnStop");
            player.robot.goCheckPoint = false;

        }
    }

    public void CreateCheckpoint()
    {
        //meshPlatillo.SetActive(true);
        baseCheckpointMesh.material = baseCheckpointMat;
        foreach (var p in palosCheckpointMesh) p.material = paloCheckpointMat;

        CheckpointManager.Instance.UpdateCheckpoint(respawnPost.position);
        activated = true;

        aniController.SetBool("Active", true);
    }
}
