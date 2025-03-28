using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum MonsterState
{
    Idle,
    Walk,
    Chasing,
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

    [Header("Monster setting")]
    public float lastAttack;

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
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Monster is not on the NavMesh!", gameObject);
            return;
        }
        SetState(MonsterState.Walk);
    }
    
    public void SetState(MonsterState stat)
    {
        state = stat;
        Debug.Log($"{gameObject.name}, {stat}, {state}");
        switch (state)
        {
            case MonsterState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case MonsterState.Walk:
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
        switch (state)
        {
            case MonsterState.Idle:
            case MonsterState.Walk:
                PassiveUpdate();
                break;
            case MonsterState.Attacking:
               // AttackingUpdate();
                break;
        }
    }

    void PassiveUpdate()
    {
        if (state == MonsterState.Walk && agent.remainingDistance < 0.1f)
        {
            SetState(MonsterState.Idle);
            Invoke("WanderToNewLocation", Random.Range(mobData.minWanderWaitTime, mobData.maxWanderWaitTime));
        }
        /*
        if (playerDistance < mobData.detectRange)
        {
            SetState(MonsterState.Attacking);
        }
*/    }
    void WanderToNewLocation()
    {
        if (state != MonsterState.Idle) return;
        Debug.Log("check");
        SetState(MonsterState.Walk);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        /*
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(1, mobData.maxWanderDistance)), out hit, mobData.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < mobData.detectRange)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(1, mobData.maxWanderDistance)), out hit, mobData.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 0) break;
        }
        return hit.position;
    */
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * mobData.maxWanderDistance;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, mobData.maxWanderDistance, NavMesh.AllAreas))
        {
            Debug.Log($"[Monster] 유효한 경로 찾음: {hit.position}");
            return hit.position;
        }

        Debug.LogWarning("[Monster] 유효한 NavMesh 위치를 찾지 못함! 다시 시도 중...");
        return transform.position;
    }
    void AttackingUpdate()
    {
        if (playerDistance < mobData.attackRange && IsPlayerInView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttack > mobData.attackRate)
            {
                lastAttack = Time.time;
                //CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
                foreach (Animator anim in animator)
                {
                    anim.speed = 1;
                    anim.SetTrigger("Attack");
                }
            }
        }
        else
        {
            if (playerDistance < mobData.detectRange)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(MonsterState.Walk);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(MonsterState.Walk);
            }
        }
    }

    bool IsPlayerInView()
    {
        Vector3 dirToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        return angle < mobData.sight * 0.5f;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            isDie = true;
            foreach (Animator anim in animator)
            {
                anim.SetTrigger("Die");
            }
            StartCoroutine(Die());
        }
    }
    
    IEnumerator Die()
    {

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
