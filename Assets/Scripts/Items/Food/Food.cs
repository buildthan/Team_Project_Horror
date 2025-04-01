using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조합이 가능 -> 인터페이스(나중에 해보자)

public abstract class Food : BaseItem
{
    // 음식은 일단 섭취가능하다
    // 총의 경우 총알을 실제로 발사하는건 총이라서 Fire메서드를 선언했으나
    // 음식의 경우 음식이 음식을 먹지는 않으니까
    // 메서드를 별도로 선언하지 않는다
}

