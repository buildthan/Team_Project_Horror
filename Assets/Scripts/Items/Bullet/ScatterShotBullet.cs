using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
