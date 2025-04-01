using UnityEngine;

public class GunView : MonoBehaviour
{
    Vector3 basePos;
    float bobSpeed = 5f;
    float bobAmountY = 0.06f;
    float bobAmountX = 0.01f;
    float timer;

    //  공격 반동용 변수
    Vector3 recoilOffset = Vector3.zero;
    Vector3 targetRecoilOffset = Vector3.zero;
    public float recoilBackAmount = 0.1f;
    public float recoilReturnSpeed = 10f;

    void Start()
    {
        basePos = transform.localPosition;
    }

    void Update()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 bobPos = basePos;

        if (isMoving)
        {
            float speed = isRunning ? bobSpeed * 1.5f : bobSpeed;
            float amountY = isRunning ? bobAmountY * 1.5f : bobAmountY;
            float amountX = isRunning ? bobAmountX * 1.5f : bobAmountX;

            timer += Time.deltaTime * speed;

            float x = Mathf.Cos(timer) * amountX;
            float y = Mathf.Abs(Mathf.Sin(timer)) * amountY;

            bobPos += new Vector3(x, y, 0);
        }
        else
        {
            timer = 0f;
        }

        //  반동 적용 (Z축 뒤로 밀림)
        targetRecoilOffset = Vector3.Lerp(targetRecoilOffset, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
        recoilOffset = Vector3.Lerp(recoilOffset, targetRecoilOffset, Time.deltaTime * recoilReturnSpeed);

        transform.localPosition = bobPos + recoilOffset;
    }

    //  외부에서 발사 시 호출
    public void PlayRecoil()
    {
        targetRecoilOffset = new Vector3(0, 0, -recoilBackAmount);
    }
}
