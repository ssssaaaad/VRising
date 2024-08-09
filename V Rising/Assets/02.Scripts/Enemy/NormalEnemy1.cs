using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public float damage;

    private float distance;
    public float patrolSpeed = 5;
    public float traceSpeed = 10;

    public float attackDelayTime = 1;
    public float attackCoolTime = 2;
    private bool isAttack = false;
    private bool canAttack = true;

    private Coroutine coroutine_BackOrigin;

    public new void InitEnemy()
    {
        base.InitEnemy();
    }

    private void ChangeState(State state)
    {
        this.state = state;

        if (coroutine_BackOrigin != null)
        {
            StopCoroutine(coroutine_BackOrigin);
            coroutine_BackOrigin = null;
        }

        switch (state)
        {
            case State.Patrol:
                navMeshAgent.speed = patrolSpeed;
                break;
            case State.Trace:
                navMeshAgent.speed = traceSpeed;
                break;
            case State.BackOrigin:
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
                Attack();
                break;
            case State.Die:
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
                Patrol();
                break;
            case State.Trace:
                Trace();
                break;
            case State.BackOrigin:
                BackOrigin();
                break;
        }
    }
    private void Patrol()
    {
        //if(Vector3.Distance(new Vector3(transform.position.x,0, transform.position.z),new Vector3(origin.position.x,0, origin.position.z)) < 1)
        //{

        //}
        forward = navMeshAgent.velocity;
        Rotate();
    }

    private void Trace()
    {
        if(target != null)
        {
            if(Vector3.Distance(transform.position, origin.position) > 30)
            {
                state = State.BackOrigin;
            }
            forward = navMeshAgent.velocity;
            Rotate();
            if (distance <= range)
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
        if (canAttack)
        {
            StartCoroutine(Coroutine_AttackDelay());
            StartCoroutine(Coroutine_AttackCool());
        }
        else
        {
            direction = (transform.position - target.position).normalized;
            float angle = Mathf.Atan2(direction.z, direction.x);
            if (movePosition_rotate)
            {
                angle += 30 * Mathf.Deg2Rad;
            }
            else
            {
                angle += -30 * Mathf.Deg2Rad;
            }
            movePosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (range - range / 3);
            movePosition += target.position;


        }
    }

    private void BackOrigin()
    {
        if(Vector3.Distance(transform.position, origin.position) < 15)
        {
            if(target != null && Vector3.Distance(target.position, transform.position) < 10)
            {
                ChangeState(State.Trace);
                return;
            }
        }
        else
        {
            forward = navMeshAgent.velocity;
            Rotate();
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
        while(hp_Current < hp_Max)
        {
            hp_Current += hp_Max * 0.1f;
            yield return new WaitForSeconds(1);
        }
    }

}
