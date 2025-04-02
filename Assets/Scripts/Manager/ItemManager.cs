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
    //public Dictionary<System.Type, List<BaseItemDataSO>> itemPool; // 비활성화 후 아이템 저장
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
    public T GetEnableItemFromPool<T>(T item) where T : BaseItem/*, new()*/
    {
        // 어떤 아이템을 가져와야하는지 매개변수로 받아야한다
        //System.Type type = typeof(T);
        /// type이 BaseItem으로 넘어온다. 이걸 실형식으로 바꿔야한다
        System.Type type = item.GetType();
        Debug.Log($"런타임 자료형: {type.Name}");

        /// 버렸다가 줍는 아이템은 이름에 (Clone)이 붙는다
        /// 삭제와 생성을 하기 때문...
        /// 저장되어 있는 이름도 (Clone)이 붙는다
        /// 그래서 이름비교할때 저장되어 있는 이름에 (Clone)을 제거하지않으면 
        /// 당연히 없다고 나온다
        /// 버릴때 삭제하지 않으면 된다

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
                        // 이것도 (Clone)붙는 경우를 대비해야할 것 같다
                        string childName = child.gameObject.name.Replace("(Clone)", "").Trim();
                        string prefabName = enableItem.name.Replace("(Clone)", "").Trim();


                        //if (child.GetType() == type && child.name == enableItem.name)
                        if (child.GetType() == type && childName == prefabName)
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

        //System.Type type = typeof(T);
        System.Type type = item.GetType();  // 자식의 자료형도 담는다


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
        // 편의를 위해 itemPool에 있는 무기를 equippedItems으로 옮긴다

        // item을 itemPool에서 검색해서 가져온다
        // 풀에서 꺼내 활성화
        BaseItem equipItem = GetEnableItemFromPool(item);
        // 혹시 여기도 (Clone)이름 붙으면 문제가 생길 것을 대비하여 제거
        //string equipName = equipItem.gameObject.name.Replace("(Clone)", "").Trim();
        string itemName = item.name.Replace("(Clone)", "").Trim();
        equipItem.name = itemName; // (Clone) 제거

        if (!equippedItems.Contains(equipItem))
        {
            equippedItems.Add(equipItem);

            // 부모를 GunHolder로 바꾼다
            equipItem.transform.SetParent(CharacterManager.Instance.Player.weaponPosition);
            equipItem.transform.localPosition = Vector3.zero;    // 로컬pos (0,0,0)
            equipItem.transform.localRotation = Quaternion.identity;   // 회전각 (0,0,0)

            CharacterManager.Instance.Player.interaction.currentWeapon = equipItem as Weapon;
            equipItem.gameObject.SetActive(true);

        }
    }
    /// <summary>
    /// 아이템 해제
    /// </summary>
    public void UnEquipItem(BaseItem item)
    {
        // equipItems에 있는 무기를 itemPool으로 옮긴다
        // 기존의 무기를 해제했다면 위치는 그냥 놔두고 비활성화해도 상관없지만
        /// 만약 버리는 경우를 대비해 부모를 itemParentTr로 바꿔야 한다
        /// itemParentTr의 자식들을 검색해서 버릴 아이템을 찾아오기 때문이다
        
        // equipItems에서 제거한다 
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
            item.gameObject.transform.SetParent(itemParentTr);

            item.gameObject.SetActive(false);  // 아이템 비활성화
            // 부모 transform은 교체하지 않는다

            ReturnItemToPool(item); // 해제된 아이템을 풀로 반환
        }
    }

    // 사용한 아이템 삭제
    public void RemoveItem(BaseItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("삭제하려는 아이템이 null입니다.");
            return;
        }

        System.Type itemType = item.GetType(); // 아이템의 실제 타입 가져오기

        // itemPool에서 해당 타입의 리스트가 있는지 확인
        if (itemPool.ContainsKey(itemType))
        {
            // 리스트에서 해당 아이템 검색
            List<BaseItem> itemList = itemPool[itemType];

            /// itemList.Contains(item)는 아이템의 이름과 자료형만 같다고 해서 true를 리턴하지 않는다
            /// 같은 것을 참조하고 있어야 한다
            //if (itemList.Contains(item))
            //{
            //    itemList.Remove(item); // 리스트에서 아이템 제거
            //    Debug.Log($"아이템 {item.name}이 itemPool에서 제거되었습니다.");
            //}

            // 이름과 자료형이 같은 아이템 찾기
            BaseItem targetItem = null;
            foreach (BaseItem listItem in itemList)
            {
                string listItemName = listItem.name.Replace("(Clone)", "").Trim();
                string itemName = item.name.Replace("(Clone)", "").Trim();

                //if (listItem.name == item.name && listItem.GetType() == item.GetType())
                if (listItemName == itemName && listItem.GetType() == item.GetType()) // 이름으로 비교하여 동일한 프리팹 확인
                {
                    targetItem = listItem;
                    break;
                }
            }

            if (targetItem != null)
            {
                itemList.Remove(targetItem); // 리스트에서 아이템 제거
                Debug.Log($"아이템 {targetItem.name}이 itemPool에서 제거되었습니다.");
            }
            else
            {
                Debug.LogWarning($"아이템 {item.name}이 itemPool에 존재하지 않습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"itemPool에 {itemType.Name} 타입의 리스트가 존재하지 않습니다.");
        }
    }
}
