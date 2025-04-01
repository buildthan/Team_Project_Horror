using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 잠긴 서랍 등을 여는 열쇠
[CreateAssetMenu(fileName = "NewDrawerKey", menuName = "Items/Key/DrawerKey")]

public class DrawerKeyDataSO : GameKeyDataSO
{
    // 이미 클래스 이름이 다르니까 굳이 다른 변수는 필요하지 않다
    // 클래스 이름으로 충분히 구분가능하다
    // 실제로 총알 구분도 그렇게 하고 있다
}
