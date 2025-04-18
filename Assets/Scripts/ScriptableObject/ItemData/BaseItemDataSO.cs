using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object를 이용한 아이템 생성
/// 종류를 상세히 구분하여 생성한다
/// 최종 프로젝트때 사용 가능한 아이템 구조 만들기
/// 
/// 구현의 우선순위
/// 1. Weapon
/// 2. Bullet
/// 3. Consumable
/// 4. 중립 아이템(열쇠, ...)
/// 
/// 최종 프로젝트에 추가해야 하는 것들
/// Armor
/// 
/// </summary>
public class BaseItemDataSO : ScriptableObject
{
    [Header("Item Info")]
    public int id;  // 아이템을 구분, json으로 저장할 때 사용
    // id 를 string으로 W0000 같은거
    // 소비하는 아이템은 C0000 같은식으로

    // 같은타입인지로 비교하면 상관이 없다..

    public string itemName;
    public string description;
    public Sprite icon;
    public GameObject prefab;/// 프리팹 정보(저장해두어야, 나중에 검색하지 않고 이를 이용하여 인스턴스를 생성)

    [Header("Stacking")]    // 아이템은 여러개 가질 수 있는 것도 있다
    public bool canStack;   // 여러개 가질 수 있는 아이템인가?
    public int maxStackAmount;  // 얼마나 많이 가질 수 있는가?

}
