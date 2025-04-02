using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 관통(Piercing)
[CreateAssetMenu(fileName = "NewPiercingBullet", menuName = "Items/Bullet/PiercingBullet")]
public class PiercingBulletDataSO : BulletDataSO
{
    public int piercingCount; // 관통력 (몇 명의 적을 관통할 수 있는지)
}

