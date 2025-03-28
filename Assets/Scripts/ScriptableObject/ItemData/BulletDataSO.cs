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


