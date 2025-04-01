using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 장착기능
/// </summary>
public class Equipment : MonoBehaviour
{
    public ItemManager itemManager;

    public Transform equipParent;   // 장비를 달아줄 위치: 오른팔
    // ItemManager가 있으니까 Equipment를 쓰지 않는다

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// 무기 장착
    /// </summary>
    public void EquipItem(BaseItem item)
    {
        if (!itemManager.equippedItems.Contains(item))
        {
            itemManager.equippedItems.Add(item);
            item.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 아이템 해제
    /// </summary>
    public void UnequipItem(BaseItem item)
    {
        if (itemManager.equippedItems.Contains(item))
        {
            itemManager.equippedItems.Remove(item);
            itemManager.ReturnItemToPool(item); // 해제된 아이템을 풀로 반환
        }
    }


}
