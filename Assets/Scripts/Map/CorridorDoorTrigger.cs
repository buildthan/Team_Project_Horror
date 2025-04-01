using UnityEngine;

public class CorridorDoorTrigger : MonoBehaviour
{
    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            RoomManager roomManager = FindAnyObjectByType<RoomManager>();

            if (roomManager != null)
            {
                roomManager.SpawnNextRoom();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("문 앞");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
