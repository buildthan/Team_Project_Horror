using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("가구 스폰")]
    public Transform[] objectSpawnPoints;
    public GameObject[] objectPrefabs;

    [Header("열쇠 스폰")]
    public Transform[] keySpawnPoints;
    public GameObject keyPrefab;

    private GameObject spawnedKey;

    private void Start()
    {
        SpawnObjects();
        SpawnKey();
    }

    private void SpawnObjects()
    {
        int spawnCount = Random.Range(3, 5);
        List<Transform> availablePoints = objectSpawnPoints.ToList();

        for (int i = 0; i < spawnCount; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Transform point = availablePoints[index];

            GameObject furniture = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)], point.position, Quaternion.identity, point);
        
            availablePoints.RemoveAt(index);  // 중복 배치 방지
        }
    }

    private void SpawnKey()
    {
        Transform point = keySpawnPoints[Random.Range(0, keySpawnPoints.Length)];
        spawnedKey = Instantiate(keyPrefab, point.position, Quaternion.identity);
    }

    //void SpawnGunAndAmmo()
    //{
    //    // 0~2 사이의 총 종류 중 랜덤 선택
    //    int index = Random.Range(0, gunPrefabs.Length);

    //    // 고른 인덱스의 총과 총알을 스폰
    //    spawnedGun = Instantiate(gunPrefabs[index], GetItemPosition(), Quaternion.identity);
    //    spawnedAmmo = Instantiate(ammoPrefabs[index], GetItemPosition(), Quaternion.identity);
    //}

    //// 오브젝트 내 스폰 포인트 중 하나를 랜덤 선택
    //// 근데 아이템이랑 건이랑 따로인데
    //Vector3 GetItemPosition()
    //{
    //    // 일단 방 안의 모든 가구를 찾고
    //    var findFurniture = GameObject.FindObjectOfType<FurnitureObject>();

    //    // 그 안의 모든 itemSpawnPoints를 찾고
    //    var spots = new List<Transform>();
    //    foreach (var f in findFurniture)
    //        spots.AddRange(f.itemSpawnPoints);

    //    if (spots.Count == 0)
    //    {
    //        Debug.Log("스폰 포인트가 없습니다");
    //        return Vector3.zero;   
    //    }

    //    // 그 중 하나를 랜덤으로 선택하여 위치 반환
    //    return spots[Random.Range(0, spots.Count)].position;
    //}
}
