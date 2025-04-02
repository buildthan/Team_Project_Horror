using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealingFood", menuName = "Items/Food/HealingFood")]
public class HealingFoodDataSO : FoodDataSO
{
    public float value;    // 체력회복량
}
