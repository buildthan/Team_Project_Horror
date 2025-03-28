using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Items/Weapon/RangedWeapon")]
public class RangedWeaponDataSO : WeaponDataSO
{
    // 총은 공격을 할 수 없다
    // 총이 가져야 할 능력치: 사거리, 연사속도, 사용 가능한 탄환의 종류
    public Bullet bulletType;   // 총알 유형

    public float range;         // 사거리
    public float fireRate;      // 연사속도(실제 공격속도)
    public float reloadTime;    // 재장전 시간
}
