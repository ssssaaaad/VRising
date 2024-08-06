using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Maja_Minion : Enemy
{
    public Maja maja;
    public float speed = 10;
    public float traceRange = 5;
    public float attackRange = 1;

    public bool onTarget = false;
    public bool respawn = false;

    private Vector3 moveDirection;
    private Vector3 moveposition;
    private float targetEnemyDistance;

    private Vector3 randomPosition;
    private float randomAngle = 70;
    private float randomDistance;
    private float randomDirection;

    private float traceTime_Max = 3;
    private float traceTime = 0;

    private float attackCooltime = 1;
    private bool attackCheck = false;


    void Update()
    {
        StateCycle();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public new void InitEnemy(Maja maja)
    {
        base.InitEnemy();
        this.maja = maja;
        target = maja.target;

        onTarget = false;
        respawn = true;
        navMeshAgent.speed = speed;
        maja.AddMinion(this);
        ResetRandomDirection();
        StartCoroutine(ChangeAngle());
        StartCoroutine(InitTimeCheck());
    }

    private void StateCycle()
    {
        switch (state)
        {
            case State.Move:
                SetMovePosition();
                break;
            case State.Death:
                maja.RemoveMinion(this);
                return;
                break;
            case State.Attack:
                Attack();
                break;
            default:
                SetMovePosition();
                break;
        }
    }


    private void ResetRandomDirection()
    {
        randomDirection = Random.value > 0.5 ? -1 : 1;
        randomDistance = Random.Range(maja.mapRadius/2, maja.mapRadius-1);
    }

    private void SetMovePosition()
    {
        if(target == null)
        {
            return;
        }

        targetEnemyDistance = Vector3.Distance(target.position, transform.position);
        if (targetEnemyDistance <= traceRange)
        {
            onTarget = true;
        }
        else
        {
            onTarget = false;
        }


        if (!onTarget || respawn)
        {
            
            moveDirection = (transform.position - maja.mapOrigin.position);
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;
            float angle = Mathf.Atan2(moveDirection.z, moveDirection.x);
            angle += (randomAngle * randomDirection) * Mathf.Deg2Rad;
            moveposition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            moveposition *= randomDistance;
            moveposition += maja.mapOrigin.position;

            traceTime += Time.deltaTime;
            if (traceTime >= traceTime_Max)
            {
                traceTime = 0;
                ResetRandomDirection();
            }
            MovePosition(moveposition);
        }
        else
        {

            MovePosition(target.position);
        }

        if (targetEnemyDistance <= attackRange)
        {
            state = State.Attack;
        }
    }

    IEnumerator ChangeAngle()
    {
        while (true)
        {
            randomAngle = Random.Range(10, 150);
            yield return new WaitForSeconds(Random.Range(1,3));
        }
    }

    IEnumerator InitTimeCheck()
    {
        yield return new WaitForSeconds(2);
        respawn = false;
    }

    private void Attack()
    {
        StopMoveTarget();
        StartCoroutine(Coroutine_Attack());
    }

    IEnumerator Coroutine_Attack()
    {
        // 공격중인지 체크
        if (attackCheck)
        {
            yield return new Break();
        }

        // 공격 딜레이
        attackCheck = true;
        // 어택 애니메이션 추가 바람
        yield return new WaitForSeconds(2);
        if(state == State.Death)
        {
            yield return new Break();
        }
        attackCheck = false;

        
        state = State.Move;
    }
}
