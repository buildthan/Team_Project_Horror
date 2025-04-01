using JetBrains.Annotations;
using UnityEngine;

public class RoomDoorTrigger : MonoBehaviour
{
    public GameObject corridor;

    public bool isPlayerNear = false;
    private bool hasReturned = false;
    
    private void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !hasReturned)
        {
            hasReturned = true;

            GameObject currentRoom = transform.root.gameObject;

            Destroy(currentRoom);
            corridor.SetActive(true);

            Debug.Log("복도로 돌아감");
        }
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
