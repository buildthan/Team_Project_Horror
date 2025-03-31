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

    public void SetState(MonsterState stat) //현재 상태 변화
    {
        state = stat;
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
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        switch (state)
        {
            case MonsterState.Idle:
            case MonsterState.Walk:
                PassiveUpdate();
                break;
            case MonsterState.Attacking:
                AttackingUpdate();
                break;
        }
        foreach (Animator anim in animator)
        {
            anim.SetBool("Move", state != MonsterState.Idle);
        }

        if(state == MonsterState.Attacking)
        {
            foreach (Animator anim in animator)
            {
                anim.SetBool("Move", false);
            }
        }
    }

    void PassiveUpdate() //비전투
    {
        if (state == MonsterState.Walk && agent.remainingDistance < 0.1f)
        {
            SetState(MonsterState.Idle);
            Invoke("WanderToNewLocation", Random.Range(mobData.minWanderWaitTime, mobData.maxWanderWaitTime));
        }
       
        if (playerDistance < mobData.detectRange)
        {
            SetState(MonsterState.Attacking);
        }
 /**/    }
    void WanderToNewLocation() //새 목적지 갱신
    {
        if (state != MonsterState.Idle) return;
        SetState(MonsterState.Walk);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation() //새 목적지 탐색
    {
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
    void AttackingUpdate() //전투시
    {
        if (playerDistance <= mobData.attackRange && IsPlayerInView())
        {
            agent.isStopped = true;
            foreach (Animator anim in animator)
            {
                anim.SetBool("Move",false);
            }
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
            if (playerDistance <= mobData.detectRange)
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
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(MonsterState.Walk);
            }
            SetState(MonsterState.Walk);
        }
    }

    bool IsPlayerInView() //플레이어가 시야 안에 있는 지
    {
        Vector3 dirToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        return angle < mobData.sight * 0.5f;
    }

    public void TakeDamage(int damage) //피격 시
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

    void OnDrawGizmos() //Scene에서 시야 범위 출력
    {
        // 시야 범위 그리기
        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward;
        float angleStep = mobData.sight / 10f;
        for (float angle = -mobData.sight / 2f; angle <= mobData.sight / 2f; angle += angleStep)
        {
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * forward;
            Gizmos.DrawLine(transform.position, transform.position + direction * mobData.detectRange);
        }

        // 범위 안에서의 원형 영역
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, mobData.detectRange);
    }

    IEnumerator Die()
    {

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
