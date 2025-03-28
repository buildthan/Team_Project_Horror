using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유탄(Grenade)
[CreateAssetMenu(fileName = "NewGrenadeBullet", menuName = "Items/Bullet/GrenadeBullet")]
public class GrenadeBulletDataSO : BulletDataSO
{
    public float explosionRadius; // 폭발 반경
    // 폭발로 인한 데미지
}