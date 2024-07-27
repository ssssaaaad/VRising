using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public interface Pattern
{
    public void InitPattern(Maja maja);
    public void ActivePattern(Vector3 direction);
}

public class Maja : Enemy
{
    public Transform mapOriginPosition;
    public Pattern[] attackPatterns;
    public Pattern teleport;
    private bool check_NormalAttck = true;
    private Vector3 movementPosition;
    public float mapRadius = 10;
    private float enemyDistance;
    private Vector3 enemyDirection;
    private Vector3 targetDirection;
    private float angle;

    private bool wall = false;
    private bool setMovePosition = false;

    private State state;

    enum State
    {
        Idle,
        Move,
        Runaway,
        Teleport,
    }

    void Awake()
    {
        InitEnemy();

        for (int i = 0; i < attackPatterns.Length; i++)
        {
            attackPatterns[i].InitPattern(this);
        }
        teleport.InitPattern(this);
    }


    protected new void InitEnemy()
    {
        base.InitEnemy();

    }

    private void PatternCycle()
    {
        if (state == State.Idle)
        {
            return;
        }
        else if (state == State.Move)
        {
            Move();
        }
        else if (state ==State.Runaway)
        {
            Runaway();
        }
        else if(state == State.Teleport)
        {
            teleport.ActivePattern(Vector3.zero);
        }
    }


    private void AttakPatter(int index)
    {
        attackPatterns[index].ActivePattern(target.position);
    }


    private void Move()
    {
        if (setMovePosition)
        {
            return;
        }

        enemyDirection = new Vector3(transform.position.x - mapOriginPosition.position.x, 0, transform.position.z - mapOriginPosition.position.z).normalized;
        angle = Mathf.Atan2(enemyDirection.z, enemyDirection.x) * Mathf.Rad2Deg;

        if(Random.value > 0.5)
        {
            angle += 70;
        }
        else
        {
            angle -= 70;
        }
        angle *= Mathf.Deg2Rad;
        movementPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (mapRadius - 1);
        movementPosition += mapOriginPosition.position;
    
        navMeshAgent.SetDestination(movementPosition);
        setMovePosition = true;
    }

    private void Runaway()
    {
        if (target == null)
            return;
        enemyDistance = Vector3.Distance(mapOriginPosition.position, transform.position);
        if (enemyDistance < mapRadius-1 && !wall)
        {
            movementPosition = transform.position + (transform.position - target.position).normalized * (mapRadius - enemyDistance);

            enemyDirection = Vector3.Cross(transform.forward, new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));

            if (enemyDistance > 0)
            {
                movementPosition += transform.right;
            }
            else
            {
                movementPosition += -transform.right;
            }
        }
        else
        {
            wall = true;
            
            // 각도 구하기
            enemyDirection = new Vector3(transform.position.x - mapOriginPosition.position.x, 0 , transform.position.z - mapOriginPosition.position.z).normalized;
            targetDirection = new Vector3(target.position.x - mapOriginPosition.position.x, 0, target.position.z - mapOriginPosition.position.z).normalized;
            angle = Mathf.Atan2(enemyDirection.z, enemyDirection.x) * Mathf.Rad2Deg;

            // 이동 방향 구하기
            Vector3 enemy_Cross = Vector3.Cross((mapOriginPosition.position - transform.position).normalized, new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));

            //if (텔레포트 사용가능)
            //{ 

                if(Vector3.Distance(transform.position, target.position) < 2)
                {
                    state = State.Teleport;
                    return;
                }    

            //}

            if (enemy_Cross.y < -0.5)
            {
                angle += 70;
            }
            else if(enemy_Cross.y > 0.5)
            {
                angle -= 70;
            }
            else
            {
                angle *= Mathf.Deg2Rad;
                movementPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (mapRadius * 0.6f);
                movementPosition += mapOriginPosition.position;
                navMeshAgent.SetDestination(movementPosition);
                return;
            }

            angle *= Mathf.Deg2Rad;
            movementPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (mapRadius - 1);
            movementPosition += mapOriginPosition.position;
        }
        navMeshAgent.SetDestination(movementPosition);
    }


}
