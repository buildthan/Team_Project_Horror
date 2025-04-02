using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bullet의 오브젝트 풀링
/// 사용한 bullet을 종류별로 저장한다
/// 
/// </summary>
public class BulletManager : MonoBehaviour
{
    // 총알의 종류별로 다른 리스트를 만든다
    // 검색속도를 줄이기 위해 Dictionary 사용
    public Dictionary<System.Type, List<Bullet>> bulletPool;

    /// <summary>
    /// bulletPool에 저장하는건 코드를 이용하여 가능하다고 하지만...
    /// 실제 오브젝트의 parent를 변경하는건 else if 뿐인가...
    /// </summary>
    public Transform normalBulletParentTr;     // 사용한 normal 총알은 여기로 모은다
    public Transform piercingBulletParentTr;   // 사용한 piercing 총알은 여기로 모은다
    public Transform scatterBulletParentTr;    // 사용한 scatter 총알은 여기로 모은다
    public Transform grenadeBulletParentTr;    // 사용한 grenade 총알은 여기로 모은다

    /// <summary>
    /// BulletManager 관련 메서드가 제네릭인 이유는
    /// bulletPool에 리플렉션을 이용하여 저장했기 때문
    /// </summary>
    int magazineIndex;  // 탄창은 몇번 인덱스인가

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new Dictionary<System.Type, List<Bullet>>();
        UIManager.Instance.gameUI.bulletManager = this;
        magazineIndex = -1;
    }

    #region magazine(탄창)
    // 총을 장착하는 경우, 그것에 맞는 총알을 가진 아이템 슬롯의 총알을 자동으로 연결한다
    // 무기 장착은 OnUpEquipButton뿐이므로 OnUpEquipButton에서 호출한다
    public void LoadBullet(RangedWeapon weapon)
    {
        RangedWeaponDataSO weaponData = weapon.weaponData;  // 총
        GameObject weaponPrefab = weaponData.prefab;    // 총알 프리팹

        BaseItemDataSO matchedBulletItem = null;

        GameObject[] slots = UIManager.Instance.gameUI.inventorySlots;
        // 인벤토리 순회하여 장착된 총에 맞는 총알 찾기
        for (int i = 0; i < slots.Length; i++)
        {
            BaseItemDataSO item = slots[i].GetComponent<ItemSlot>().itemData;

            if (item.prefab == weaponPrefab) // 장착된 무기와 같은 프리팹인지 확인
            {
                continue; // 무기 자체는 건너뜀
            }

            if (item is RangedWeaponDataSO rangedWeaponData && rangedWeaponData.bulletType == weaponData.bulletType)
            {
                // 현재까지 찾은 가장 작은 인덱스 업데이트
                if (magazineIndex == -1 || i < magazineIndex)
                {
                    magazineIndex = i;
                    matchedBulletItem = item;
                }
            }
        }

        if (matchedBulletItem != null)
        {
            // 총과 총알 연결
            weapon.bullet = matchedBulletItem.prefab.GetComponent<Bullet>();
            Debug.Log($"총알 {matchedBulletItem.itemName}을(를) {weaponData.itemName}과 연결");
        }
        else
        {
            Debug.LogWarning($"해당 총({weaponData.itemName})에 맞는 총알이 인벤토리에 없습니다.");
        }
    }


    #endregion


    #region bullet의 오브젝트 풀링
    /// <summary>
    /// 총알을 발사할 때 총알을 풀에서 가져옴 (없으면 생성)
    /// 잔여 총알의 수를 줄이는 것은 총알을 발사하는 곳에서 한다
    /// </summary>
    public T GetEnableBulletFromPool<T>() where T : Bullet, new()
    {
        System.Type type = typeof(T);

        if (!bulletPool.ContainsKey(type))
        {
            bulletPool[type] = new List<Bullet>();
        }

        if (bulletPool[type].Count > 0)
        {
            Bullet bullet = bulletPool[type][0];
            bulletPool[type].RemoveAt(0);

            // 플레이어의 총으로 위치를 옮긴다
            // 부모를 플레이어의 무기로 바꾸어야할듯

            bullet.gameObject.SetActive(true);
            return (T)bullet;
        }
        else
        {
            // 새로운 총알 생성
            T newBullet = new T();
            return newBullet;
        }
    }

    /// <summary>
    /// 사용한 총알을 풀에 반환
    /// </summary>
    public void ReturnBulletToPool<T>(T bullet) where T : Bullet
    {
        System.Type type = typeof(T);

        if (!bulletPool.ContainsKey(type))
        {
            bulletPool[type] = new List<Bullet>();
        }

        bullet.gameObject.SetActive(false);
        bulletPool[type].Add(bullet);
    }
    #endregion
}
