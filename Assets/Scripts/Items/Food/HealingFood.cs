using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFood : Food
{
    public HealingFoodDataSO healingFoodData;
    public override BaseItemDataSO GetItemData()
    {
        return healingFoodData;
    }
}
