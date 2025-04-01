using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bullet의 오브젝트 풀링
/// 사용한 bullet을 종류별로 저장한다
/// </summary>
public class BulletManager : MonoBehaviour
{
    // 총알의 종류별로 다른 리스트를 만든다
    // 검색속도를 줄이기 위해 Dictionary 사용
    public Dictionary<System.Type, List<Bullet>> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new Dictionary<System.Type, List<Bullet>>();
        UIManager.Instance.gameUI.bulletManager = this;
    }

    /// <summary>
    /// 총알을 발사할 때 총알을 풀에서 가져옴 (없으면 생성)
    /// 잔여 총알의 수를 줄이는 것은 총알을 발사하는 곳에서 한다
    /// </summary>
    public T GetEnableBullet<T>() where T : Bullet, new()
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
}
