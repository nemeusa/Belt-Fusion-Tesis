using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] AudioClip collectSound;
    [SerializeField] ParticleSystem collectEffects;
    [SerializeField] int points = 1;

    public GameObject[] crystalsMeshes;

    private void Awake()
    {
        crystalsMeshes[Random.Range(0, crystalsMeshes.Length)].SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            GameManager.instance.PlaySound(collectSound);
            
            var p = Instantiate(collectEffects, player.transform.position, Quaternion.identity);

            p.Play();

            Destroy(p.gameObject, 1);

            player.CountCrystals(points);

            Destroy(gameObject);
        }
    }
}
