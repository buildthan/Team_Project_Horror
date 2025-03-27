using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSecond = 20f;
    public float staminaRegenPerSecond = 10f;
    public float regenDelay = 2f;

    private float lastSprintTime;
    private PlayerController controller;

    public bool IsSprintingAllowed => currentStamina > 0f;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        currentStamina = maxStamina;
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

        // 스태미나가 0이면 강제로 스프린트 끄기
        if (currentStamina <= 0f && controller.IsSprinting())
        {
            controller.ForceStopSprint();
        }

        // 디버그 출력
        Debug.Log($"[PlayerCondition] 현재 스태미나: {currentStamina:F1} / {maxStamina}");
    }

    void DrainStamina()
    {
        currentStamina -= staminaDrainPerSecond * Time.deltaTime;
    }

    void RegenStamina()
    {
        currentStamina += staminaRegenPerSecond * Time.deltaTime;
    }
}