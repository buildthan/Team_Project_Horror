using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// bullet은 weapon이 아니다
// 총알 "하나" 객체를 생성
[CreateAssetMenu(fileName = "NewBullet", menuName = "Items/Bullet")]

public class BulletDataSO : BaseItemDataSO
{
    public float speed; // 총알이 날아가는 속도
    public int damage;  // 공격력(bullet의 종류가 다르면 damage를 주는 양상은 다르다)
}


// 총알의 종류를 구분해야하므로 세분화한다
// 보통(Normal) 
[CreateAssetMenu(fileName = "NewNormalBullet", menuName = "Items/Bullet/NormalBullet")]
public class NormalBulletDataSO : BulletDataSO
{
}

// 산탄(ScatterShot)
[CreateAssetMenu(fileName = "NewScatterShotBullet", menuName = "Items/Bullet/ScatterShotBullet")]
public class ScatterShotBulletDataSO : BulletDataSO
{
    public int scatterCount;   // 퍼지는 탄환 개수
    public float spreadAngle;  // 탄환이 퍼지는 각도 범위
}

// 관통(Piercing)
[CreateAssetMenu(fileName = "NewPiercingBullet", menuName = "Items/Bullet/PiercingBullet")]
public class PiercingBulletDataSO : BulletDataSO
{
    public int piercingCount; // 관통력 (몇 명의 적을 관통할 수 있는지)
}

// 유탄(Grenade)
[CreateAssetMenu(fileName = "NewGrenadeBullet", menuName = "Items/Bullet/GrenadeBullet")]
public class GrenadeBulletDataSO : BulletDataSO
{
    public float explosionRadius; // 폭발 반경
    // 폭발로 인한 데미지
}