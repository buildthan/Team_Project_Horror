using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShotBullet : Bullet
{
    /// <summary>
    /// ScriptableObject에 있는 변수는 사용하지 않는다
    /// </summary>
    //public int scatterCount;   // 퍼지는 탄환 개수
    //public float spreadAngle;  // 탄환이 퍼지는 각도 범위
    public ScatterShotBulletDataSO bulletDataSO;

    public override BaseItemDataSO GetItemData()
    {
        return bulletDataSO; /// 부모 타입(BaseItemDataSO)으로 반환(업캐스팅)
    }



    public override void Activate(Vector3 startPosition, Vector3 direction)
    {
        base.Activate(startPosition, direction);

        /// 로직 잘못되었다. 수정해야한다.
        // 산탄총알 발사 로직
        //for (int i = 0; i < scatterCount; i++)
        //{
        //    float angleX = Random.Range(-spreadAngle, spreadAngle);
        //    float angleY = Random.Range(-spreadAngle, spreadAngle);
        //    Vector3 spreadDir = Quaternion.Euler(angleX, angleY, 0) * direction;
        //    Debug.Log($"ScatterShotBullet fired in direction {spreadDir}");
        //}
    }
    // 닿은 물체에 데미지를 주고 산탄총알 하나씩 깎는다

}
