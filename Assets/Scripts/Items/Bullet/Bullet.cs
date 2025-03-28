using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Bullet : MonoBehaviour
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

public class NormalBullet : Bullet
{
    public int bulletDamage; // 총알의 공격력

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

public class ScatterShotBullet : Bullet
{
    public int scatterCount;   // 퍼지는 탄환 개수
    public float spreadAngle;  // 탄환이 퍼지는 각도 범위

    public override void Activate(Vector3 startPosition, Vector3 direction)
    {
        base.Activate(startPosition, direction);

        // 산탄총알 발사 로직
        for (int i = 0; i < scatterCount; i++)
        {
            float angleX = Random.Range(-spreadAngle, spreadAngle);
            float angleY = Random.Range(-spreadAngle, spreadAngle);
            Vector3 spreadDir = Quaternion.Euler(angleX, angleY, 0) * direction;
            Debug.Log($"ScatterShotBullet fired in direction {spreadDir}");
        }
    }
    // 닿은 물체에 데미지를 주고 산탄총알 하나씩 깎는다

}

public class PiercingBullet : Bullet
{
    public int piercingCount; // 관통력 (몇 명의 적을 관통할 수 있는지)

    public override void Activate(Vector3 startPosition, Vector3 direction)
    {
        base.Activate(startPosition, direction);

        // 관통 총알 발사 로직


        // 총알이 발사되었을 때 여러 명의 적을 관통하는 효과를 추가
        Debug.Log("PiercingBullet fired with piercing power: " + piercingCount);

        // 하나 닿을때마다 piercingCount가 1씩 줄어들고, 0이 되거나 벽에 닿으면 사라진다(오브젝트 풀링)
    }

    // 닿은 물체에 데미지를 주고 piercingCount을 하나 줄인다
    public override void OnHit(Collider collider)
    {
        // 관통할 수 있는 적이 있을 때만 관통
        if (piercingCount > 0)
        {
            piercingCount--;
            base.OnHit(collider);
            Debug.Log("PiercingBullet passed through and now has " + piercingCount + " penetrations left.");
        }
        else
        {
            Debug.Log("PiercingBullet has no more penetrations left.");
        }
    }

}

public class GrenadeBullet : Bullet
{
    public float explosionRadius; // 폭발 반경
    public int explosionDamage;   // 폭발로 인한 데미지

    public override void Activate(Vector3 startPosition, Vector3 direction)
    {
        base.Activate(startPosition, direction);

        // 유탄 발사 로직
        // 무엇이든 닿으면 폭발한다

        // 총알이 발사되었을 때 폭발하는 효과를 추가
        Debug.Log("ExplosiveBullet fired with explosion radius: " + explosionRadius + " and explosion damage: " + explosionDamage);
    }
    // 닿으면 폭발(Circle collider? 만든 후 collider 내부에 있으면 데미지를 준다)
    public override void OnHit(Collider collider)
    {
        // 유탄이 닿으면 폭발 처리
        base.OnHit(collider);

        // 폭발 범위 내 적들에게 데미지 처리 (예시로 원형 범위에서 폭발)
        Collider[] hitColliders = Physics.OverlapSphere(collider.transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(explosionDamage);
                Debug.Log($"Explosion damage dealt to {hitCollider.name}");
            }
        }
    }
}