using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float attackDamage = 20f; // 플레이어에게 줄 공격력
    public int hp = 100;             // 체력

    // Interaction에서 우클릭 시 호출됨
    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log($"[Enemy] {amount} 데미지 받음! 현재 체력: {hp}");

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("[Enemy] 사망 처리됨!");
        gameObject.SetActive(false); // 또는 Destroy(gameObject);
    }
}