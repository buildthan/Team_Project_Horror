using UnityEngine;


public abstract class Weapon : BaseItem

{
    public WeaponData data;
    private float lastFireTime;

    public bool CanFire()
    {
        return Time.time >= lastFireTime + data.fireRate;
    }

    public void Fire()
    {
        lastFireTime = Time.time;
    }

    public int GetDamage()
    {
        return data.damage;
    }
}

