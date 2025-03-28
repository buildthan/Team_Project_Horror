using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Spawnable
{
    public GameObject prefab;
    public float spawnChance;
}

public class SpawnManager : MonoBehaviour
{
    [Header("스폰 위치")]
    public Transform[] itemSpawnPoints;
    public Transform[] objectSpawnPoints;

    [Header("프리팹 목록")]
    public List<Spawnable> itemPrefabs;
    public List<Spawnable> objectPrefabs;

    [Header("현재 방에서 몇 개 생성할지")]
    public int itemCount = 3;
    public int objectCount = 2;

    public void Spawn()
    {
        SpawnPrefab(itemSpawnPoints, itemPrefabs, itemCount);
        SpawnPrefab(objectSpawnPoints, objectPrefabs, objectCount);
    }

    void SpawnPrefab(Transform[] points, List<Spawnable> prefabs, int count)
    {
        var shuffled = points.OrderBy(x => Random.value).Take(count);

        foreach (var point in shuffled)
        {
            var prefab = GetRandomByChance(prefabs);
            if (prefab != null)
                Instantiate(prefab, point.position, point.rotation, this.transform);
        }
    }

    GameObject GetRandomByChance(List<Spawnable> list)
    {
        float total = list.Sum(s => s.spawnChance);
        float rand = Random.Range(0f, total);
        float current = 0f;

        foreach (var s in list)
        {
            current += s.spawnChance;
            if (rand <= current)
                return s.prefab;
        }

        return null;
    }
}
