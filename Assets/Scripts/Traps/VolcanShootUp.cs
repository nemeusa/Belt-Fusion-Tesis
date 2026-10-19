using UnityEngine;

public class VolcanShootUp : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform spawnPoint;   // si lo dejas vacío usa este mismo objeto
    [SerializeField, Min(0.01f)] float cooldown = 1.5f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            timer -= cooldown; // conserva el sobrante para que el ritmo sea constante
            Shoot();
        }
    }

    void Shoot()
    {
        Transform point = spawnPoint != null ? spawnPoint : transform;
        Instantiate(ballPrefab, point.position, Quaternion.identity);
    }
}
