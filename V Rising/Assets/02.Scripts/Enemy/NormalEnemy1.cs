using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalEnemy1 : Enemy
{
    public enum State
    {
        Idle,
        Patrol,
        Trace,
        Attack,
        Die,
        BackOrigin,
        Drain
    }

    public State state;
    public float range;
    public float traceRange = 30;
    public float damage;

    private float distance;
    public float patrolSpeed = 5;
    public float traceSpeed = 10;

    public float attackDelayTime = 1;
    public float attackCoolTime = 2;
    private bool isAttack = false;
    private bool canAttack = true;

    private Coroutine coroutine_BackOrigin;
    Vector3 modeolDir;

    private List<Transform> visibleTargets = new List<Transform>();
    public float viewRadius;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float viewAngle;
    private Transform targetTransform;

    public float idleTime = 2;
    private float timeCheck = 0;

    public Transform attackPosition;
    public AttackCollision attackCollision;


    private void Awake()
    {
        InitEnemy();
    }

    private void Update()
    {
        StateUpdate();
        if (state != State.Die && state != State.Drain)
        {
            Rotate();
        }
    }

    public new void InitEnemy()
    {
        base.InitEnemy();
        animator.SetBool("alive", alive);
        effectTrigger.callBackAction += () => 
        { 
            AttackCollision temp = Instantiate(attackCollision, attackPosition.position, model.transform.rotation, transform);
            temp.InitAttack(damage, true);
            temp.DestoryCollision(0.5f);
        };
        drainFinishEvent += () =>
        {
            UpdateHP(-hp_Max, target); 
            animator.SetBool("alive", alive);
            animator.SetTrigger("Death");
            target = null;
            StopMoveTarget();
        };

        navMeshAgent.acceleration = 70;
    }

    private void ChangeState(State state)
    {
        this.state = state;
        timeCheck = 0;

        if (coroutine_BackOrigin != null && (state == State.Attack || state == State.Trace))
        {
            StopCoroutine(coroutine_BackOrigin);
            coroutine_BackOrigin = null;
        }

        animator.SetBool("Run", false);
        switch (state)
        {
            case State.Idle:
                break;
            case State.Patrol:
                navMeshAgent.speed = patrolSpeed;
                Patrol();
                break;
            case State.Trace:
                navMeshAgent.speed = traceSpeed;
                enemyUI.ActiveFindIcon();
                animator.SetBool("Run", true);
                enemyGroup.SetTarget(target);
                break;
            case State.BackOrigin:
                target = null;
                MovePosition(origin.position);
                coroutine_BackOrigin = StartCoroutine(Coroutine_BackOrigin());
                break;
            case State.Attack:
                if(Random.value > 0.5f)
                {
                    movePosition_rotate = false;
                }
                else
                {
                    movePosition_rotate = true;
                }
                break;
            case State.Die:
                if (canDrain)
                {
                    animator.SetBool("alive", alive);
                    animator.SetTrigger("Groggy");
                    target = null;
                    StopMoveTarget();
                }
                else
                {
                    enemyUI.InactiveUI();
                    animator.SetBool("alive", alive);
                    animator.SetTrigger("Death");
                    target = null;
                    StopMoveTarget();
                }
                break;
            case State.Drain:
                canDrain = false;
                StopMoveTarget();
                break;
        }
    }

    private void StateUpdate()
    {
        if (state == State.Die)
        {
            return;
        }
        if(hp_Current == 0)
        {
            ChangeState(State.Die);
            return;
        }
        if (drain && state != State.Drain)
        {
            ChangeState(State.Drain);
            return;
        }

        if(targetTransform != null)
        {
            target = targetTransform;
        }

        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);
            if (Vector3.Distance(target.position, origin.position) > traceRange && !isAttack)
            {
                ChangeState(State.BackOrigin);
                return;
            }

            forward = (target.position - transform.position).normalized;
        }
        else
        {
            forward = navMeshAgent.velocity;
        }
        switch (state)
        {
            case State.Idle:
                timeCheck += Time.deltaTime;

                if (target != null)
                {
                    ChangeState(State.Trace);
                    return;
                }
                else if (timeCheck >= idleTime)
                {
                    ChangeState(State.Patrol);
                }
                FindVisibleTargets();
                break;
            case State.Patrol:
                if(target != null)
                    ChangeState(State.Trace);
                SetAnimtion_Movement();
                Patrol();
                FindVisibleTargets();
                break;
            case State.Trace:
                Trace();
                SetAnimtion_Movement();
                break;
            case State.BackOrigin:
                BackOrigin();
                SetAnimtion_Movement();
                break;
            case State.Attack:
                Attack();
                if(!isAttack)
                {
                    modeolDir = model.transform.InverseTransformDirection(forward);
                    animator.SetFloat("Horizontal", modeolDir.x);
                    animator.SetFloat("Vertical", modeolDir.z);
                }
                else
                {
                    animator.SetFloat("Horizontal", 0);
                    animator.SetFloat("Vertical", 0);
                }
                break;
        }
    }

    private void SetAnimtion_Movement()
    {
        modeolDir = model.transform.InverseTransformDirection(navMeshAgent.velocity.normalized);
        animator.SetFloat("Horizontal", modeolDir.x);
        animator.SetFloat("Vertical", modeolDir.z);
    }

    private void SetPatrolIndex()
    {
        if (patrolPoints.Length == 0)
        {
            ChangeState(State.Idle);
            return;
        }

        float distance_Max = float.MaxValue;
        float distance = 0;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (distance < distance_Max)
            {
                patrolIndex = i;
                distance_Max = distance;
            }
        }

    }

    private void Patrol()
    {
        if(patrolPoints.Length == 0)
        {
            ChangeState(State.Idle);
            return;
        }

        if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) < 1)
        {
            if(++patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
        }
        MovePosition(patrolPoints[patrolIndex].position);
    }

    private void Trace()
    {
        if(target != null)
        {
            if (Vector3.Distance(target.position, origin.position) > traceRange)
            {
                ChangeState(State.BackOrigin);
            }
            if (distance > range)
            {
                MovePosition(target.position);
            }
            else
            {
                ChangeState(State.Attack);
            }
        }
    }

    private Vector3 direction;
    private Vector3 movePosition;
    private bool movePosition_rotate = false;
    private void Attack()
    {
        if(target == null && !isAttack)
        {
            ChangeState(State.BackOrigin);
            return;
        }

        if (distance > range && !isAttack)
        {
            ChangeState(State.Trace);
            return;
        }

        if (canAttack)
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("AttackNum", Random.Range(0, 3));
            StopMoveTarget();
            StartCoroutine(Coroutine_AttackDelay());
            StartCoroutine(Coroutine_AttackCool());
            ChangeState(State.Attack);
        }
        else if(!isAttack)
        {
            direction = (transform.position - target.position).normalized;
            float angle = Mathf.Atan2(direction.z, direction.x);
            if (movePosition_rotate)
            {
                angle += Mathf.Deg2Rad * 10;
            }
            else
            {
                angle += -Mathf.Deg2Rad * 10;
            }
            movePosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (range - range / 4);
            movePosition += target.position;

            MovePosition(movePosition);
        }
    }

    private void BackOrigin()
    {
        if (Vector3.Distance(transform.position, origin.position) < 15)
        {
            if(target != null && Vector3.Distance(target.position, transform.position) < 10)
            {
                ChangeState(State.Trace);
                return;
            }
        }
        else
        {
            target = null;
        }  

        if(Vector3.Distance(transform.position, origin.position) < 1)
        {
            StopMoveTarget();
            ChangeState(State.Patrol);
        }
    }

    private IEnumerator Coroutine_AttackDelay()
    {
        if (!canAttack)
            yield break;
        while (distance < range - (range/2))
        {
            if (!alive)
                yield break;
            MovePosition(target.position);
            yield return null;
        }

        isAttack = true;
        yield return new WaitForSeconds(attackDelayTime);
        isAttack = false;
    }
    private IEnumerator Coroutine_AttackCool()
    {
        if (!canAttack)
            yield break;
        canAttack = false;
        yield return new WaitForSeconds(attackCoolTime);
        canAttack = true;
    }
    private IEnumerator Coroutine_BackOrigin()
    {
        while(hp_Current != hp_Max)
        {
            if (hp_Current == 0)
                yield break;
            UpdateHP(hp_Max * 0.1f, null);
            yield return new WaitForSeconds(1);
        }
        coroutine_BackOrigin = null;
    }

    protected void FindVisibleTargets()
    {
        visibleTargets.Clear();
        // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
            if (Vector3.Angle(model.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        if (visibleTargets.Count > 0)
        {
            targetTransform = visibleTargets[0];
        }
        else
        {
            targetTransform = null;
        }
    }
}
