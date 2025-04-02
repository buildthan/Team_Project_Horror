using System.Collections;
using UnityEngine;

// 방 프리팹 세 개 중에 랜덤하게 한 개 생성
public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public Transform roomSpawnPoint;  // 복도 끝
    private GameObject currentRoom;
    public GameObject corridor;

    public void SpawnNextRoom()
    {
        if (currentRoom != null)
            Destroy(currentRoom);  // 현재 방 삭제

        GameObject randomRoom = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        currentRoom = Instantiate(randomRoom, roomSpawnPoint.position, roomSpawnPoint.rotation);

        SetActiveCorrider();
    }

    public void SetActiveCorrider()
    {
        corridor.SetActive(!corridor.activeInHierarchy);
    }
}
