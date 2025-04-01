using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 잠긴 문을 여는 열쇠로, 스테이지 클리어에 필요하다
/// </summary>
[CreateAssetMenu(fileName = "NewDoorKey", menuName = "Items/Key/DoorKey")]
public class DoorKeyDataSO : GameKeyDataSO
{
    //// 문을 여는 열쇠인지 확인
    //public bool isDoorKey;  // 스테이지 문을 여는 키인지 여부
    // 이미 클래스 이름이 다르니까 bool 변수로 구분할 필요 없다

    // BaseItemDataSO에 필요한 변수들이 이미 있어서 굳이 여기에 선언할 것이 없다

}
