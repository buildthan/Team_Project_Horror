using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public MeleeWeaponDataSO weaponData;

    public virtual void Attack(Transform origin)
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
}
