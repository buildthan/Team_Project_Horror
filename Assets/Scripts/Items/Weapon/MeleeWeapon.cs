using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public MeleeWeaponDataSO weaponData;

    public override BaseItemDataSO GetItemData()
    {
        return weaponData; /// 부모 타입(BaseItemDataSO)으로 반환(업캐스팅)
    }



    public override bool CanFire()
    {
        throw new System.NotImplementedException();
    }

    // 실제로는 Attack이다
    public override void Fire()
    {
        //Collider[] hitEnemies = Physics.OverlapSphere(origin.position, 1.5f);
        //foreach (Collider enemy in hitEnemies)
        //{
        //    if (enemy.CompareTag("Enemy"))
        //    {
        //        enemy.GetComponent<Enemy>().TakeDamage(meleePower);
        //    }
        //}
    }

    public override int GetDamage()
    {
        throw new System.NotImplementedException();
    }
}
