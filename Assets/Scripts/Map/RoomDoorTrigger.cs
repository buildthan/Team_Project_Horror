using JetBrains.Annotations;
using UnityEngine;

public class RoomDoorTrigger : MonoBehaviour
{
    public bool isPlayerNear = false;
    private bool hasReturned = false;
    
    private void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !hasReturned)
        {
            GameObject currentRoom = transform.root.gameObject;
            hasReturned = true;
            BackCorridor();
            Destroy(currentRoom);
        }
    }

    void BackCorridor()
    {
        RoomManager roomManager = FindAnyObjectByType<RoomManager>();

        roomManager.SetActiveCorrider();


        Debug.Log("복도로 돌아감");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }
}
