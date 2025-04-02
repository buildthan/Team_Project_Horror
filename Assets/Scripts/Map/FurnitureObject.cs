using UnityEngine;
using System.Collections.Generic;

public class FurnitureObject : MonoBehaviour
{
    [Header("스폰 위치")]
    public List<Transform> itemSpawnPoints;
    public List<Transform> gunSpawnPoints;

    [Header("프리팹")]
    public GameObject[] itemPrefabs;
    public GameObject[] gunPrefabs;
    public GameObject[] ammoPrefabs;

    [Header("스폰 확률")]
    [Range(0f, 1f)] public float itemSpawnChance = 0.5f;

    private void Start()
    {
        SpawnItem();
        SpawnGunAndAmmo();
    }

    // 일반 아이템 확률적 스폰
    public void SpawnItem()
    {
        if (itemSpawnPoints.Count == 0 || itemPrefabs.Length == 0)
            return;

        if (Random.value > itemSpawnChance)
            return;

        Transform point = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count)];
        GameObject item = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        Instantiate(item, point.position, point.rotation);
    }

    // 총과 총알 세트 무조건 스폰
    public void SpawnGunAndAmmo()
    {
        // 총 + 총알 세트로 스폰되는 조건
        if (gunSpawnPoints.Count < 2 || gunPrefabs.Length == 0 || ammoPrefabs.Length != gunPrefabs.Length)
        {
            Debug.Log("총/총알 스폰 조건 미충족");
            return;
        }

        // 총 종류 중 한 개 랜덤 선택
        int index = Random.Range(0, gunPrefabs.Length);
        GameObject gun = gunPrefabs[index];
        GameObject ammo = ammoPrefabs[index]; 

        List<Transform> available = new List<Transform>(gunSpawnPoints);
        Transform gunPoint = available[Random.Range(0, available.Count)];
        available.Remove(gunPoint);
        Transform ammoPoint = available[Random.Range(0, available.Count)];

        Instantiate(gun, gunPoint.position, gunPoint.rotation);
        Instantiate(ammo, ammoPoint.position, ammoPoint.rotation);  
    }
}