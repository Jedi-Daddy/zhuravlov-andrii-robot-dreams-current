using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint; // Точка выхода пули
    public GameObject bulletPrefab; // Префаб пули
    public LayerMask hitLayers; // Слои, по которым можно стрелять
    public float bulletSpeed = 20f; // Скорость пули
    public float maxRange = 100f; // Дальность Raycast и SphereCast
    public float explosionRadius = 2f; // Радиус действия SphereCast
    public float explosionForce = 500f; // Сила разлета объектов от SphereCast

    private int fireMode = 1; // 1 - Raycast, 2 - SphereCast, 3 - Object Spawn

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            fireMode++;
            if (fireMode > 3) fireMode = 1; // Зацикливаем режимы
            Debug.Log("Режим стрельбы: " + GetFireModeName());
        }
    }

    string GetFireModeName()
    {
        return fireMode switch
        {
            1 => "Raycast",
            2 => "SphereCast",
            3 => "Object Spawn",
            _ => "Unknown"
        };
    }

    void Shoot()
    {
        Debug.Log("Стрельба запущена! Режим: " + GetFireModeName());

        if (fireMode == 1)
        {
            RaycastShoot();
        }
        else if (fireMode == 2)
        {
            SphereCastShoot();
        }
        else if (fireMode == 3)
        {
            SpawnBullet();
        }
    }

    void RaycastShoot()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red, 2f);
        Debug.Log("Пускаем Raycast...");

        if (Physics.Raycast(ray, out hit, maxRange, hitLayers))
        {
            Debug.Log("Попадание в: " + hit.collider.name);

            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * 500f);
            }
        }
        else
        {
            Debug.Log("Луч не попал никуда!");
        }
    }

    void SphereCastShoot()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.green, 2f);
        Debug.Log("Пускаем SphereCast...");

        if (Physics.SphereCast(ray, explosionRadius, out hit, maxRange, hitLayers))
        {
            Debug.Log("Сфера задела: " + hit.collider.name);

            Collider[] colliders = Physics.OverlapSphere(hit.point, explosionRadius);
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, hit.point, explosionRadius);
                }
            }
        }
        else
        {
            Debug.Log("Сфера не задела цели!");
        }
    }

    void SpawnBullet()
    {
        Debug.Log("Создаём пулю...");

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Ошибка: `bulletPrefab` или `firePoint` не установлены!");
            return;
        }

        // Создаём пулю в точке firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet == null)
        {
            Debug.LogError("Ошибка: пуля не создана!");
            return;
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Ошибка: у пули нет Rigidbody!");
            return;
        }

        // Запускаем пулю вперёд
        rb.velocity = firePoint.forward * bulletSpeed;

        Debug.Log("Пуля запущена!");
    }
}
