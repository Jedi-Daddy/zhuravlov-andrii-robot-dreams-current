using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int maxEnemies = 10;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Префаб врага не назначен! Спаун не запущен.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("Список точек спауна пуст! Добавь хотя бы одну точку.");
            return;
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawnedEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, randomPoint.rotation);
        spawnedEnemies.Add(enemy);

        // Удаляем из списка, если враг уничтожен
        enemy.GetComponent<EnemyAI>()?.StartCoroutine(RemoveWhenDead(enemy));

        Debug.Log("Враг заспаунился в точке: " + randomPoint.name);
    }

    private IEnumerator RemoveWhenDead(GameObject enemy)
    {
        while (enemy != null)
        {
            yield return null;
        }
        spawnedEnemies.Remove(enemy);
    }

    // Гизмо для визуализации точек спауна
    private void OnDrawGizmos()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(point.position, 0.5f);
            }
        }
    }
}
