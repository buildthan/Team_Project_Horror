using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : Bullet
{
    //public int piercingCount; // 관통력 (몇 명의 적을 관통할 수 있는지)
    public PiercingBulletDataSO bulletDataSO;

    public override BaseItemDataSO GetItemData()
    {
        return bulletDataSO; /// 부모 타입(BaseItemDataSO)으로 반환(업캐스팅)
    }


    public override void Activate(Vector3 startPosition, Vector3 direction)
    {
        base.Activate(startPosition, direction);

        // 관통 총알 발사 로직


        // 총알이 발사되었을 때 여러 명의 적을 관통하는 효과를 추가
        Debug.Log("PiercingBullet fired with piercing power: " + bulletDataSO.piercingCount);

        // 하나 닿을때마다 piercingCount가 1씩 줄어들고, 0이 되거나 벽에 닿으면 사라진다(오브젝트 풀링)
    }

    // 닿은 물체에 데미지를 주고 piercingCount을 하나 줄인다
    public override void OnHit(Collider collider)
    {
        // 관통할 수 있는 적이 있을 때만 관통
        if (bulletDataSO.piercingCount > 0)
        {
            bulletDataSO.piercingCount--;
            base.OnHit(collider);
            Debug.Log("PiercingBullet passed through and now has " + bulletDataSO.piercingCount + " penetrations left.");
        }
        else
        {
            Debug.Log("PiercingBullet has no more penetrations left.");
        }
    }

}

