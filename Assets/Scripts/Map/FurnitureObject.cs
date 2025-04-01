using UnityEngine;
using System.Collections.Generic;

public class FurnitureObject : MonoBehaviour
{
    public List<Transform> itemSpawnPoints;

    public void TrySpawnItem(GameObject itemPrefab, float spawnChance = 1f)
    {
        if (itemSpawnPoints.Count == 0 || Random.value > spawnChance)
            return;

        Transform point = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count)];
        Instantiate(itemPrefab, point.position, point.rotation);
    }
}