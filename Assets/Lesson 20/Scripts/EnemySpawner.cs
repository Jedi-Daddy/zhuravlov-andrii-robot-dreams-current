using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;   // Префаб врага
    [SerializeField]
    private Transform spawnPoint;     // Точка, где будут спауниться враги (можно заменить на случайную точку)
    [SerializeField]
    private float spawnInterval = 10f; // Периодичность спауна врагов (в секундах)

    private void Start()
    {
        // Запускаем корутину спауна врагов
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Спауним врага
            SpawnEnemy();

            // Ждём указанное время перед следующим спауном
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            // Спавним врага в точке spawnPoint
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Враг заспаунился!");
        }
        else
        {
            Debug.LogError("Префаб врага не назначен!");
        }
    }
}
