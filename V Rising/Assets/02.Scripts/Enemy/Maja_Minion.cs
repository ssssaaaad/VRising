using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.AI;

public class Maja_Minion : Enemy
{
    public enum State
    {
        Move,
        Attack,
        MajaMainSkill_2,
        Death,
    }

    public State state;
    public Maja maja;
    public AttackCollision attackCollision_Prefab;
    public Transform attackPosition;
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

    public float damage = 15;
    public float damage_Min = 15;
    public float damage_Max = 30;

    private AttackCollision attackCollision;

    void Update()
    {
        StateCycle();
        Rotate();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public new void InitEnemy(Maja maja, float damage)
    {
        base.InitEnemy();
        this.maja = maja;
        target = maja.target;
        SetDamage(damage);
        onTarget = false;
        respawn = true;
        navMeshAgent.speed = speed;
        maja.AddMinion(this);
        ResetRandomDirection();
        StartCoroutine(ChangeAngle());
        StartCoroutine(InitTimeCheck());

    }

    public void SetDamage(float dmg)
    {
        if (dmg > damage_Max)
        {
            dmg = damage_Max;
        }
        else if (dmg < damage_Min)
        {
            dmg = damage_Min;
        }
        damage = dmg;
    }

    private void StateCycle()
    {
        if (state == State.Death)
        {
            return;
        }
        if (!alive)
        {
            state = State.Death;
        }

        switch (state)
        {
            case State.Move:
                SetMovePosition();
                break;
            case State.Death:
                animator.SetTrigger("Die");
                maja.RemoveMinion(this);
                Collider[] colliders = GetComponentsInChildren<Collider>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].isTrigger = true;
                }
                navMeshAgent.isStopped = true;
                model.transform.DOMoveY(model.transform.position.y - 5, 0.5f).SetEase(Ease.InQuad);
                Destroy(gameObject, 3);
                return;
                break;
            case State.Attack:
                Attack();
                Rotate();
                break;
            case State.MajaMainSkill_2:
                return;
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
            moveposition = target.position;
            MovePosition(moveposition);
        }

        forward = new Vector3(moveposition.x - model.position.x, 0, moveposition.z - model.position.z).normalized;

        if (targetEnemyDistance <= attackRange && !respawn)
        {
            if (state == State.MajaMainSkill_2)
                return;
            state = State.Attack;
        }
    }

    public void SetPosition_MajaMainSkill2(Vector3 startPosition)
    {
        state = State.MajaMainSkill_2;
        MovePosition(startPosition);
    }
    public void ActiveMajaMainSkill2(float minionSpeed)
    {
        state = State.MajaMainSkill_2;
        speed = minionSpeed;
        navMeshAgent.speed = speed;
        MovePosition(maja.transform.position);
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
        if (state == State.MajaMainSkill_2)
            yield break;
        respawn = true;
        state = State.Move;
        yield return new WaitForSeconds(2);
        respawn = false;
    }

    private void Attack()
    {
        StartCoroutine(Coroutine_Attack());
    }

    IEnumerator Coroutine_Attack()
    {
        // 공격중인지 체크
        if (attackCheck || !alive)
        {
            yield break;
        }
        // 공격 딜레이
        attackCheck = true;
        forward = new Vector3(target.position.x - model.position.x, model.position.y, target.position.z - model.position.z).normalized;
        yield return new WaitForSeconds(0.15f);
        animator.SetTrigger("Attack");
        attackCollision = Instantiate(attackCollision_Prefab);
        attackCollision.transform.SetParent(attackPosition);
        attackCollision.transform.localPosition = Vector3.zero;
        attackCollision.transform.localRotation = Quaternion.Euler(Vector3.zero);
        attackCollision.InitAttack(damage, true);
        Destroy(attackCollision.gameObject, 0.5f);
        yield return new WaitForSeconds(0.5f);
        if (!alive)
        {
            yield break;
        }
        StopMoveTarget();

        yield return new WaitForSeconds(0.5f);
        if (!alive)
        {
            yield break;
        }
        attackCheck = false;

        StartCoroutine(InitTimeCheck());
    }
}
