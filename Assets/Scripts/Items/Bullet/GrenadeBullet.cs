using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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