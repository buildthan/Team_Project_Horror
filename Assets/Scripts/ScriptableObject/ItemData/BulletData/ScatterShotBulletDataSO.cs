using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 산탄(ScatterShot)
[CreateAssetMenu(fileName = "NewScatterShotBullet", menuName = "Items/Bullet/ScatterShotBullet")]
public class ScatterShotBulletDataSO : BulletDataSO
{
    public int scatterCount;   // 퍼지는 탄환 개수
    public float spreadAngle;  // 탄환이 퍼지는 각도 범위
}

