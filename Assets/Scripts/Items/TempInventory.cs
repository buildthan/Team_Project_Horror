using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TempInventory : MonoBehaviour
{
    public TempInventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        // 20개의 슬롯을 생성
        slots = new TempInventorySlot[20];

        // 슬롯을 배열에 할당 (만약 객체가 씬에 이미 존재한다면, GetComponent로 할당)
        for (int i = 0; i < slots.Length; i++)
        {
            // 임시로 GameObject에서 컴포넌트 가져오기 (이 부분은 필요에 따라 수정)
            slots[i] = new TempInventorySlot();  // 실제로는 슬롯들을 게임 오브젝트에서 가져오는 것이 일반적입니다.
        }
    }

}
