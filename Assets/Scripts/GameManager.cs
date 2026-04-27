using System.Collections;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]public PlayerController player;
    public BoostContainer BoostContainer;
    private bool winGame;
    [SerializeField] TransitionCanvas trans;

    private void Awake()
    {
       instance = this;
    }

    private void Start()
    {
        CheckpointManager.Instance.UpdateCheckpoint(player.transform.position);
    }

    public void WinGame()
    {
        winGame = true;
        player.winGame = true;

    }

    public void Death(GameObject target)
    {
        //trans.Transition();
        //CheckpointManager.Instance.Respawn(target);
        StartCoroutine(DeathCorou(target));
    }

    IEnumerator DeathCorou(GameObject target)
    {
        trans.Transition();
        yield return new WaitForSeconds(0.5f);

        CheckpointManager.Instance.Respawn(target);
        player.DefaultPlayer();
    }
}
