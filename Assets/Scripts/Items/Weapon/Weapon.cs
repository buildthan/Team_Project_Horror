using UnityEngine;



public abstract class Weapon : BaseItem
{
    public abstract bool CanFire();

    // RangedWeapon은 Fire를 하지만
    // MeleeWeapon은 실제로 Fire하지는 않지만, 이름을 통일했다
    public abstract void Fire();

    public abstract int GetDamage();
}

