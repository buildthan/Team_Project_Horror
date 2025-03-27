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

        // ���¹̳��� 0�̸� ������ ������Ʈ ����
        if (currentStamina <= 0f && controller.IsSprinting())
        {
            controller.ForceStopSprint();
        }

        // ����� ���
        Debug.Log($"[PlayerCondition] ���� ���¹̳�: {currentStamina:F1} / {maxStamina}");
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