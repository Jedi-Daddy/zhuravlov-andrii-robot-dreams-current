using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;   // ������ �����
    [SerializeField]
    private Transform spawnPoint;     // �����, ��� ����� ���������� ����� (����� �������� �� ��������� �����)
    [SerializeField]
    private float spawnInterval = 10f; // ������������� ������ ������ (� ��������)

    private void Start()
    {
        // ��������� �������� ������ ������
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // ������� �����
            SpawnEnemy();

            // ��� ��������� ����� ����� ��������� �������
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            // ������� ����� � ����� spawnPoint
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("���� �����������!");
        }
        else
        {
            Debug.LogError("������ ����� �� ��������!");
        }
    }
}
