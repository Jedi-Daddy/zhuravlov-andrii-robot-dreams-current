using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    public GameObject coinPrefab;
    public float coinOffsetY = 0.5f; // на сколько выше пола ставить монету

    public void DropCoin(Vector3 dropPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(dropPosition + Vector3.up * 10f, Vector3.down, out hit, 10f))
        {
            Vector3 spawnPosition = hit.point + Vector3.up * coinOffsetY;
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            // Если Raycast не попал - спавним как есть
            Instantiate(coinPrefab, dropPosition, Quaternion.identity);
        }
    }
}
