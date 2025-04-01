using UnityEngine;

public class GunView : MonoBehaviour
{
    Vector3 basePos;
    float bobSpeed = 5f;      // 흔들림 속도
    float bobAmountY = 0.06f; // 위아래 흔들림
    float bobAmountX = 0.01f; // 좌우 흔들림 (줄였음)
    float timer;

    void Start()
    {
        basePos = transform.localPosition;
    }

    void Update()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            float speed = isRunning ? bobSpeed * 1.5f : bobSpeed;
            float amountY = isRunning ? bobAmountY * 1.5f : bobAmountY;
            float amountX = isRunning ? bobAmountX * 1.5f : bobAmountX;

            timer += Time.deltaTime * speed;

            float x = Mathf.Cos(timer) * amountX;
            float y = Mathf.Abs(Mathf.Sin(timer)) * amountY; // 위아래 bounce 느낌

            transform.localPosition = basePos + new Vector3(x, y, 0);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, basePos, Time.deltaTime * 5f);
            timer = 0f;
        }
    }
}
