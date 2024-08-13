using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
        BackOrigin
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

    private void Awake()
    {
        InitEnemy();
    }

    private void Update()
    {
        StateUpdate();
        Rotate();
        if (c)
        {
            c = false;
            UpdateHP(-1, player);
        }
    }

    public bool c = false;
    public Transform player;

    public new void InitEnemy()
    {
        base.InitEnemy();
        animator.SetBool("alive", alive);
    }

    private void ChangeState(State state)
    {
        this.state = state;

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
                break;
            case State.Trace:
                navMeshAgent.speed = traceSpeed;
                animator.SetBool("Run", true);
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
                animator.SetBool("alive", alive);
                animator.SetTrigger("Death");
                StopMoveTarget();
                break;
        }
    }

    private void StateUpdate()
    {
        if (state == State.Die)
            return;


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
                if (target != null)
                    ChangeState(State.Trace);
                break;
            case State.Patrol:
                if(target != null)
                    ChangeState(State.Trace);
                SetAnimtion_Movement();
                Patrol();
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
    private void Patrol()
    {
        //if(Vector3.Distance(new Vector3(transform.position.x,0, transform.position.z),new Vector3(origin.position.x,0, origin.position.z)) < 1)
        //{

        //}
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
            ChangeState(State.Idle);
        }
    }

    private IEnumerator Coroutine_AttackDelay()
    {
        if (!canAttack)
            yield break;
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

}
