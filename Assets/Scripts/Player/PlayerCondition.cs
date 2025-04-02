using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSecond = 20f;
    public float staminaRegenPerSecond = 10f;
    public float regenDelay = 2f;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    private float lastSprintTime;
    private PlayerController controller;

    public bool IsSprintingAllowed => currentStamina > 0f;

    private float lastDamageTime;
    public float damageCooldown = 1f;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (controller == null) return;

        if (controller.IsSprinting() && IsSprintingAllowed)
        {
            DrainStamina();
            lastSprintTime = Time.time;
        }
        else
        {
            if (Time.time - lastSprintTime >= regenDelay)
                RegenStamina();
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (currentStamina <= 0f && controller.IsSprinting())
        {
            controller.ForceStopSprint();
        }

        //Debug.Log($"[PlayerCondition] 스태미나: {currentStamina:F1} / {maxStamina}, 체력: {currentHealth:F1} / {maxHealth}");
    }

    private void FixedUpdate() //플레이어 Hp UI 정보 업데이트용
    {
        UIManager.Instance.gameUI.UpdatePlayerHpIndicator(currentHealth,maxHealth);
    }

    void DrainStamina()
    {
        currentStamina -= staminaDrainPerSecond * Time.deltaTime;
    }

    void RegenStamina()
    {
        currentStamina += staminaRegenPerSecond * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        UIManager.Instance.GetDamagedUI();
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        UIManager.Instance.InvokeGameOverUI();
        Debug.Log("[PlayerCondition] 플레이어 사망!");
    }

    private void OnTriggerStay(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && Time.time - lastDamageTime > damageCooldown)
        {
            lastDamageTime = Time.time;
            TakeDamage(enemy.attackDamage);

            Debug.Log($"[PlayerCondition] Enemy와 충돌 중! {enemy.attackDamage} 데미지 받음. 현재 체력: {currentHealth}");
        }
    }
}