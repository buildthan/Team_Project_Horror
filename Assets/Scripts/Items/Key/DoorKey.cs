using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : GameKey
{
    public DoorKeyDataSO gameKeyData;

    public override BaseItemDataSO GetItemData()
    {
        return gameKeyData; // BaseItemDataSO로 업캐스팅 후 반환
    }
}
