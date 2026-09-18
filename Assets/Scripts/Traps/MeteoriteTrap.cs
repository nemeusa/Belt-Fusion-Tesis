using UnityEngine;

public class MeteoriteTrap : MonoBehaviour
{


    [Header("Configuración de Disparo")]
    public Transform[] spawnPoint;       // Punto de origen de las balas (delante de la torreta)
    public float fireRate = 1f;         // Tiempo en segundos entre cada disparo
    public float bulletForce = 20f;     // Fuerza/velocidad del proyectil

    [Header("Prefabs de Balas")]
    public GameObject bulletTypeA;     // Primer tipo de bala
    public GameObject bulletTypeB;     // Segundo tipo de bala

    private float nextTimeToFire = 0f;
    private bool useTypeA = true;      // Controla la alternancia

    void Update()
    {
        // Dispara automáticamente según el fireRate
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / fireRate);
            Shoot();
        }
    }

    void Shoot()
    {
        if (spawnPoint == null) return;

        int countBullets = 0;

        // 1. Seleccionar el prefab correspondiente según el turno
        int probBullet = Random.Range(1, spawnPoint.Length + 1);

        GameObject selectedBulletPrefab = null;

        // 2. Instanciar la bala en la posición y rotación del spawnPoint
        foreach (var s in spawnPoint)
        {
            countBullets++;

            if (probBullet == countBullets)
                selectedBulletPrefab = bulletTypeB;
            else 
                selectedBulletPrefab = bulletTypeA;

                GameObject bullet = Instantiate(selectedBulletPrefab, s.position, s.rotation);

            // 3. Aplicar fuerza hacia adelante (Eje Z local del spawnPoint)
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-s.forward * bulletForce, ForceMode.Impulse);
            }

        }
        // 4. Alternar el estado para el próximo disparo
        useTypeA = !useTypeA;
    }
}
