//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RangedWeapon : Weapon
//{
//    public RangedWeaponDataSO weaponData;   

//    public Bullet bullet;    // 사용하는 Bullet의 종류

//    public override BaseItemDataSO GetItemData()
//    {
//        return weaponData; /// 부모 타입(BaseItemDataSO)으로 반환(업캐스팅)
//    }

    public override bool CanFire()
    {
        return Time.time >= weaponData.lastFireTime + weaponData.fireRate;
    }

    public override void Fire()
    {
        weaponData.lastFireTime = Time.time;
    }

    public override int GetDamage()
    {
        return weaponData.damage;
    }



//    // 임시
//    //public TempInventory inventory; // 인벤토리
//    //public TempInventorySlot inventorySlot; // Bullet이 들어있는 인벤토리 슬롯(탄창의 역할)

//    // 플레이어가 무기를 장착하면 호출되는 메서드
//    // 인벤토리의 슬롯을 순차적으로 검색하여 
//    // 사용 가능한 Bullet이 슬롯에 있는지 검색한다
//    //public virtual void FindCompatibleBulletSlot()
//    //{
//    //    // UIManager를 통해 inventory를 접근할 듯
//    //    // 지금은 없으니 TempInventory를 이용한다
//    //    for (int i = 0; i < inventory.slots.Length; i++)
//    //    {
//    //        // bullet 타입이 일치하는지 확인
//    //        // inventory.slots[i].GetType()로 하면 안된다
//    //        // 실제 슬롯이 들고 있는 데이터는 스프라이트, 아이템의 타입이 될 것이며
//    //        // 슬롯에 있는 아이템의 타입과 비교해야함. 슬롯의 타입이 아니다
//    //        if (bullet.GetType() == inventory.slots[i].GetType())
//    //        {
//    //            // 동일한 타입이므로 해당 슬롯을 사용
//    //            /// 업캐스팅이 적용되면 안되기 때문에 반드시 디버그로 확인할 것
//    //            inventorySlot = inventory.slots[i];
//    //            Debug.Log($"Found compatible {bullet.GetType().Name} slot.");
//    //            return;
//    //        }
//    //    }
//    //}


//    // 플레이어가 공격하면 호출되는 메서드
//    // 총알을 발사한다    
//    public virtual void Fire(Transform origin)
//    {
//        weaponData.description = "asd";

//        //baseItemData.description = "asdf";

    


//        //if (bulletType == null)
//        //{
//        //    Debug.LogWarning("No bullet assigned to this weapon.");
//        //    return;
//        //}

//        //Bullet newBullet = Instantiate(bulletType, origin.position, origin.rotation);
//        //newBullet.Fire(range);
//    }
//}
