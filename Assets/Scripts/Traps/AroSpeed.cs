using UnityEngine;

public class AroSpeed : MonoBehaviour
{
    [SerializeField] float multiplicatorSpeed = 1.5f;

    [SerializeField] Vector3 rotVelocity = new Vector3(0, 0, 90); // grados por segundo en cada eje

    [SerializeField] Transform meshObj;

    [SerializeField] GameObject particlesSnow;

    [SerializeField] AudioClip soundSnow;

    void Update()
    {
        meshObj.Rotate(rotVelocity * Time.deltaTime, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.speed *= multiplicatorSpeed;

            if (soundSnow != null)
            GameManager.instance.PlaySound(soundSnow);
            if (particlesSnow != null)
                Instantiate(particlesSnow, transform.position, Quaternion.identity);



        }
    }


}
