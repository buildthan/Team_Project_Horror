using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject[] roomPrefabs; // 테마별 방 3개 정도 생각 중
    public Transform spawnPosition;
    private int currentStage = 1;  // 현재 몇 번째 방인지

    // 게임 시작 시 방 1개 생성
    // 탈출 시 SpawnNextRoom 호출
    public void SpawnNextRoom()
    {
        // 랜덤한 방 프리팹 선택
        GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

        // 방 생성
        GameObject newRoom = Instantiate(roomPrefab, spawnPosition.position, Quaternion.identity);

        // 방 안의 아이템/가구 랜덤 배치
        SpawnManager spawner = newRoom.GetComponentInChildren<SpawnManager>();

        if (spawner != null)
        {
            spawner.itemCount = Mathf.Clamp(2 + currentStage, 2, 10); // 방이 깊어질수록 아이템 많아짐
            spawner.Spawn();
        }

        currentStage++;
    }
}
