using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageable
{
    void TakeDamage(int amount);
}


/// <summary>
/// 공격테스트를 위한 임시 몬스터
/// </summary>
public class TempMonster : MonoBehaviour, IDamageable
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int amount)
    {
        throw new System.NotImplementedException();
    }
}
