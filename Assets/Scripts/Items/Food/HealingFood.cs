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

    public override void UseItem()
    {
        // 플레이어의 체력에 더한다
        Debug.Log(CharacterManager.Instance.Player.condition.currentHealth);

        CharacterManager.Instance.Player.condition.currentHealth = Mathf.Min(
                    CharacterManager.Instance.Player.condition.currentHealth + healingFoodData.value,
        CharacterManager.Instance.Player.condition.maxHealth
            );
        Debug.Log(CharacterManager.Instance.Player.condition.currentHealth);

        /// 정상적으로 사용한 아이템은 삭제한다
        

    }
}
