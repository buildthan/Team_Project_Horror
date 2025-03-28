using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public RangedWeaponDataSO weaponData;

    public virtual void Fire(Transform origin)
    {
        //if (bulletType == null)
        //{
        //    Debug.LogWarning("No bullet assigned to this weapon.");
        //    return;
        //}

        //Bullet newBullet = Instantiate(bulletType, origin.position, origin.rotation);
        //newBullet.Fire(range);
    }
}
