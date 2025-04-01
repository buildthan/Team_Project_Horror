using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 인벤토리 안에 있는 아이템을 저장
/// 장착한 무기의 정보를 저장한다
/// 인벤토리를 관리하는 GameUI와 겹치지 말아야 한다
/// </summary>
public class ItemManager : MonoBehaviour
{
    // 아이템 종류별로 다른 pool을 적용
    // 검색속도를 줄이기 위해 Dictionary 사용
    //public Dictionary<System.Type, List<BaseItemDataSO>> itemPool; // 비활성화된 아이템 저장 (오브젝트 풀링)
    /// BaseItemDataSO은 씬 내에서 활성화, 비활성화가 불가능하다
    public Dictionary<System.Type, List<BaseItem>> itemPool; // 비활성화된 아이템 저장 (오브젝트 풀링)
    public List<BaseItem> equippedItems; // 장착한 아이템 리스트

    // Start is called before the first frame update
    void Start()
    {
        itemPool = new Dictionary<System.Type, List<BaseItem>>();
        equippedItems = new List<BaseItem>();
        UIManager.Instance.gameUI.itemManager = this;
    }

    // 아이템이 인벤토리에 있는지 확인하고, 인벤토리에 있으면 비활성화
    // 장착한 아이템은 다시 활성화

    /// <summary>
    /// 아이템을 가져오기 (없으면 새로 생성)
    /// </summary>
    public T GetItem<T>() where T : BaseItem, new()
    {
        System.Type type = typeof(T);

        if (!itemPool.ContainsKey(type))
        {
            itemPool[type] = new List<BaseItem>();
        }

        if (itemPool[type].Count > 0)
        {
            BaseItem item = itemPool[type][0];
            itemPool[type].RemoveAt(0);
            item.gameObject.SetActive(true);
            return (T)item;
        }
        else
        {
            T newItem = new T(); // 새로운 아이템 생성
            return newItem;
        }
    }

    /// <summary>
    /// 사용한 아이템을 풀에 반환
    /// </summary>
    public void ReturnItem<T>(T item) where T : BaseItem
    {
        System.Type type = typeof(T);

        if (!itemPool.ContainsKey(type))
        {
            itemPool[type] = new List<BaseItem>();
        }

        item.gameObject.SetActive(false);
        itemPool[type].Add(item);
    }

    /// <summary>
    /// 아이템 장착
    /// </summary>
    public void EquipItem(BaseItem item)
    {
        if (!equippedItems.Contains(item))
        {
            equippedItems.Add(item);
            item.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 아이템 해제
    /// </summary>
    public void UnequipItem(BaseItem item)
    {
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
            ReturnItem(item); // 해제된 아이템을 풀로 반환
        }
    }
}
