using System;
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

    public Transform itemParentTr;  // 플레이어의 아이템을 모으는 parent

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
    /// 슬롯에서 호출이다
    /// 그렇다면.. 슬롯에서 데이터를 가져온 다음 그걸로 아이템을 알아내야겠지?
    /// </summary>
    public T GetEnableItemFromPool<T>(T item) where T : BaseItem, new()
    {
        // 어떤 아이템을 가져와야하는지 매개변수로 받아야한다
        System.Type type = typeof(T);

        // Dictionary에서 T를 키로 하는 List에서 검색
        // 있을 수 없는 일이지만, key에 해당하는 List가없다면
        if (!itemPool.ContainsKey(type))
        {
            // 리스트가 없는게 말이 안된다            
            // 따라서 예외처리한다
            throw new InvalidOperationException($"아이템 풀에 {type.Name} 타입이 존재하지 않습니다.");
        }

        if (itemPool[type].Count > 0)
        {
            BaseItem enableItem;
            // itemPool[type]에서 검사해서 해당 아이템을 찾고
            for (int i = 0; i < itemPool[type].Count; i++)
            {
                /// 테스트 해봐야한다, name이 다를 수 있어
                if (itemPool[type][i].GetType() == type && itemPool[type][i].name == item.name)
                {
                    enableItem = itemPool[type][i];
                    // 2. itemParentTr의 자식 중에서 같은 아이템 찾기
                    BaseItem[] childItems = itemParentTr.GetComponentsInChildren<BaseItem>(true);
                    foreach (var child in childItems)
                    {
                        if (child.GetType() == type && child.name == enableItem.name)
                        {
                            child.gameObject.SetActive(true);
                            itemPool[type].RemoveAt(i);
                            return (T)child;
                        }
                    }
                }
            }
            // 여기까지 왔으면 아이템이 없다
            return null;
        }
        else
        {
            // itemPool[type]에 아무것도 없으면 null 리턴
            return null;
        }
    }
    /// <summary>
    /// 아이템을 풀에 반환
    /// </summary>
    public void ReturnItemToPool<T>(T item) where T : BaseItem
    {
        System.Type type = typeof(T);

        if (!itemPool.ContainsKey(type))
        {
            itemPool[type] = new List<BaseItem>();
        }
        item.gameObject.transform.SetParent(itemParentTr);
        item.gameObject.SetActive(false);
        itemPool[type].Add(item);
    }


    /// <summary>
    /// 아이템 장착
    /// 장착 가능한 아이템이 있는 경우 
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
            ReturnItemToPool(item); // 해제된 아이템을 풀로 반환
        }
    }
}
