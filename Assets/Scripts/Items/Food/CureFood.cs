using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureFood : Food
{
    public CureFoodDataSO cureFoodData;

    public override BaseItemDataSO GetItemData()
    {
        return cureFoodData;
    }

    public override void UseItem()
    {
        throw new System.NotImplementedException();
    }
}
