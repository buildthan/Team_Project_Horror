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
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("[PlayerCondition] 플레이어 사망!");
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            TakeDamage(enemy.attackDamage);
        }
    }
}