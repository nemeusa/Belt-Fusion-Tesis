using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;
    public RobotFollow robot;
    public BoostContainer BoostContainer;
    private bool winGame;
    [SerializeField] TransitionCanvas trans;
    [SerializeField] Animator startAniCanvas;

    AudioSource audioSource;

    private void Awake()
    {
       instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        CheckpointManager.Instance.UpdateCheckpoint(player.transform.position);
    }

    public void WinGame()
    {
        StartCoroutine(EndLevel());
    }

    IEnumerator EndLevel()
    {
        startAniCanvas.SetBool("Win", true);
        yield return new WaitForSeconds(1.1f);

        winGame = true;
        player.winGame = true;
        SceneManager.LoadScene(NextLevel._nextLevel);
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);

    }

    public void Death(GameObject target, float deathDuration, AudioClip deathSound, ParticleSystem deathEffect)
    {
        Time.timeScale = 1;
        if (deathSound != null)
        PlaySound(deathSound);

        if (deathEffect != null)
        {
            var a = Instantiate(deathEffect, player.transform.position, deathEffect.transform.rotation);
            a.Play();
            Destroy(a, 3);
        }
        //trans.Transition();
        //CheckpointManager.Instance.Respawn(target);
        StartCoroutine(DeathCorou(target, deathDuration));
    }

    IEnumerator DeathCorou(GameObject target, float deathDuration)
    {
        player.DeathPlayer();
        yield return new WaitForSeconds(deathDuration);
        trans.Transition();
        yield return new WaitForSeconds(0.5f);

        CheckpointManager.Instance.Respawn(target);
        yield return new WaitForSeconds(0.2f);
        player.RespawnPlayer();
    }

    public void PauseGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
