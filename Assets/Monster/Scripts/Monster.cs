using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "New Monster")]

public class Monster : ScriptableObject
{
    [SerializeField] private float hp;
    [SerializeField] private float attack;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;    
    [SerializeField] private float attackRate;
    [SerializeField] private float attackRange;
    [SerializeField] private float sight;
}
