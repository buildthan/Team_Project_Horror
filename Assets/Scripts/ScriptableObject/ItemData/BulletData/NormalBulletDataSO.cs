using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총알의 종류를 구분해야하므로 세분화한다
// 보통(Normal) 
[CreateAssetMenu(fileName = "NewNormalBullet", menuName = "Items/Bullet/NormalBullet")]
public class NormalBulletDataSO : BulletDataSO
{
    //public int bulletDamage; // 총알의 공격력
}
