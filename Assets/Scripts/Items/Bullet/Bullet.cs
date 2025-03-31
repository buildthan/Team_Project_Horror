using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Bullet : BaseItem
{

    /// <summary>
    /// 현실에서는 총알을 발사하면 실제로는 총알이 상대방을 공격하지만
    /// FPS에서는 raycast를 이용해 총에서 빛을 쏘아 닿으면 즉시 공격처리를 한다
    /// 
    /// 하지만 여기서는 총알의 종류에 따라 여러 방식을 구현한다
    /// 보통(Normal)총알은 raycast를 이용해 공격하므로 따로 효과를 발생시키지 않는다
    /// 산탄(Shot)총알은 특정한 개수만큼 탄을 실제로 생성하여 공격한다. 
    /// 관통(Penetrate)총알은 특정한 수 만큼 몬스터를 관통하거나 벽에 닿아야 사라진다
    /// 유탄(Grenade)는 무언가에 닿으면 폭발한다
    /// 
    /// 총알의 종류에 따라 총알의 공격양상은 다르다
    /// 
    /// 총알을 하나 소모하는 것은 공통이다
    /// 그 외 총알의 효과는 각자 클래스에서 구현한다
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="dir"></param>
    public virtual void Activate(Vector3 startPos, Vector3 dir)
    {
    }


    // 충돌 시 데미지 처리
    public virtual void OnHit(Collider collider)
    {
        // 기본적인 데미지 처리 로직
        if (collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(10); // 임의의 데미지 값
            Debug.Log("Damage dealt to: " + collider.name);
        }
    }
}


