using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    public NormalBulletDataSO normalBulletDataSO;

    public override void Activate(Vector3 startPosition, Vector3 direction)
    {
        base.Activate(startPosition, direction);
        // Normal총알은 생성하지 않고 그냥 개수를 하나씩 줄인다
        // 이미 총에서 raycast로 공격을 처리했기 때문에 굳이 만들 필요 없다
        // raycast의 대상을 연결해서 해당 대상에게 데미지만 준다

        Debug.Log("NormalBullet activated and flying in direction: " + direction);
    }

    public override void OnHit(Collider collider)
    {
        // 산탄총알의 특성상 하나씩 깎는다
        base.OnHit(collider);
    }

}

