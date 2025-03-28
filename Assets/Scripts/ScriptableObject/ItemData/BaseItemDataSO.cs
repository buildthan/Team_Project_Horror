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
    public string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;
}
