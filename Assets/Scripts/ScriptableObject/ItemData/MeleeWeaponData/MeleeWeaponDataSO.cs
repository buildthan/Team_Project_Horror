using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Items/Weapon/MeleeWeapon")]
public class MeleeWeaponDataSO : WeaponDataSO
{
    [Header("Common Melee Weapon Info")]
    public int meleePower;     // 단거리 무기 공격력
    public float swingRate;    // 연타속도(RangedWeapon의 fireRate와 이름을 비슷하게 만들었다) 
}
