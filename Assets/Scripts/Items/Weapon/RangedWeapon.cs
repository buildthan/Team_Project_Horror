using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public RangedWeaponDataSO weaponData;

    public Bullet bullet;    // 사용하는 Bullet의 종류
    private float lastFireTime = -999f;

    // 추상클래스 구현
    public override BaseItemDataSO GetItemData()
    {
        return weaponData; /// 부모 타입(BaseItemDataSO)으로 반환(업캐스팅)
    }

    public override bool CanFire()
    {
        return Time.time >= lastFireTime + weaponData.fireRate;
    }

    public override void Fire()
    {
        lastFireTime = Time.time;
    }

    public override int GetDamage()
    {
        return weaponData.damage;
    }
}
