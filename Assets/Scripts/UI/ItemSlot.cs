using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // 슬롯이 가지고 있는 것은 
    // BaseItemData가 되어야한다
    // 아이템 종류와 상관 없이 아이템 정보를 불러와야하니까
    public BaseItemDataSO itemData;

    //public GameObject ItemPrefab;
    // GameObject를 넣는건 비추천
    // sprite와 아이템의 Data만 가져와야한다
    
    // sprite는 Slot의 자식의 icon에 대입한다
    public Image icon; // 자식의 icon을 연결
    public TextMeshProUGUI quatityText;  // 수량표시 Text

    public GameUI inventory;   // UI 인벤토리 정보

    public int index;   // 몇번째 슬롯인가?
    public bool equipped;   // 장착했는가?
    public int quantity;    // 몇개?

    // UI(슬롯 한 칸) 업데이트를 위한 함수
    // AddItem으로 아이템 data를 전달받으면, 자동 호출
    // 아이템데이터에서 필요한 정보를 각 UI에 표시
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = itemData.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;   // 0이면 표시안하고 1 이상만 표시

    }
    // 슬롯에서 아이템을 비우는 경우(버리거나, 사용을 했거나) 자동 호출
    // UI(슬롯 한 칸)에 정보가 없을 때 UI를 비워주는 함수
    public void Clear()
    {
        itemData = null;
        icon.gameObject.SetActive(false);   // icon은 꺼야한다
        quatityText.text = string.Empty;
    }
    // 슬롯을 클릭했을 때 호출되는 이벤트함수.
    public void OnClickButton()
    {
        // 인벤토리의 SelectItem 호출, 현재 슬롯의 인덱스만 전달.
        inventory.SelectItem(index);
    }



}
