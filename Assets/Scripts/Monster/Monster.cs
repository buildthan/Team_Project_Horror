using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum MonsterState
{
    Idle,
    Move,
    Attacking
}

public class Monster : MonoBehaviour
{

    public MonsterData mobData;
    public bool isDie = false;
    private float hp;
    private float attack;
    private float walkSpeed;
    private float runSpeed;
    private float playerDistance;

    private NavMeshAgent agent;
    private MonsterState state;

    private Animator[] animator;
    private void Awake()
    {
        animator = GetComponentsInChildren<Animator>();
        hp = mobData.GetInitHp();
        attack = mobData.GetInitAttack();
        walkSpeed =mobData.GetWalkSpeed();
        runSpeed = mobData.GetRunSpeed();
    }
    void Start()
    {
        SetState(MonsterState.Idle);
    }
    public void SetState(MonsterState stat)
    {
        state = stat;
        switch (state)
        {
            case MonsterState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case MonsterState.Move:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case MonsterState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }
        foreach (Animator anim in animator)
        {
            anim.speed = agent.speed / walkSpeed;
        }
    }
    private void Update()
    {
        foreach (Animator anim in animator)
        {
            anim.SetBool("Move", state != MonsterState.Idle);
        }
    }
    public void TakePhysicalDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            isDie = true;
            StartCoroutine(Die());
        }
        switch (state)
        {
            case MonsterState.Idle:
            case MonsterState.Move:
 
                break;
            case MonsterState.Attacking:

                break;
        }
    }
    
    IEnumerator Die()
    {

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
