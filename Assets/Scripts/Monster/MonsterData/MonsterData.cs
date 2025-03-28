using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobType
{
    Zombie,
    Creature
}

[CreateAssetMenu(fileName = "Monster", menuName = "new Monster Data")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private float hp;
    [SerializeField] private float attack;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    public float detectRange;
    public float attackRate;
    public float attackRange;
    public float sight;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    public float maxWanderDistance;

    public MobType mobType;
    public float GetInitHp() { return hp; }

    public float GetInitAttack() { return attack; }

    public float GetWalkSpeed() { return walkSpeed; }

    public float GetRunSpeed() { return runSpeed; }

    public float GetAttackRate() { return attackRate; }

    public float GetAttackRange() { return attackRange; }

    public float GetSight(){ return sight; }
}
