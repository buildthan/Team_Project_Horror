using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : BaseUI
{
    public GameObject inventory;
    public GameObject[] inventorySlots;
    public GameObject inventorySlotPrefab;
    protected override UIState GetUIState()
    {
        return UIState.Game;
    }

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && this.gameObject.activeSelf)
        {
            if (inventory.activeSelf == false)
            {
                Time.timeScale = 0;
                inventory.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                inventory.SetActive(false);
            }
        }
    }

    public void Start()
    {
        inventorySlots = new GameObject[20]; //아이템 슬롯 생성
        //추후에 아이템 데이터를 받아와서 적용하는 것으로 바꿀 것.

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = Instantiate(inventorySlotPrefab, this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0));
        }
    }

}
