using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public delegate bool PatternDelay();

public abstract class Pattern : MonoBehaviour
{
    public abstract void InitPattern(Maja maja);
    public abstract void ActivePattern(Vector3 direction);
    public abstract bool CooltimeCheck();

    protected abstract bool GetPatternDelay();

    public float delayTime;
    protected bool patterDelay;

    protected  abstract IEnumerator PatternDelayTime();
}


public class Maja : Enemy
{
    public Transform mapOriginPosition;
    public List<Pattern> attackPatterns;
    public Pattern teleport;

    public float mapRadius = 10;
    public float runawayDistance = 5;
    public float teleportDistance = 2;

    private bool check_NormalAttck = true;
    private Vector3 movementPosition;
    private float enemyDistance;
    private Vector3 enemyDirection;
    private Vector3 targetDirection;
    private Vector3 enemy_Cross;
    private float angle;

    private bool wall = false;
    private bool setMovePosition = false;

    public float attackCooltime_Max = 2; 
    private float attackCooltime_Current = 2;
    private float routineTime = 0.2f;

    public PatternDelay PatternDelay;
    private Coroutine patterCycle;

    void Awake()
    {
        InitEnemy();
    }

    protected new void InitEnemy()
    {
        base.InitEnemy();

        attackPatterns = new List<Pattern>();

        Pattern pattern = GetComponent<Maja_AttackPattern1>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_AttackPattern2>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_AttackPattern3>();
        attackPatterns.Add(pattern);

        for (int i = 0; i < attackPatterns.Count; i++)
        {
            attackPatterns[i].InitPattern(this);
        }

        teleport = GetComponent<Maja_Teleport>();
        teleport.InitPattern(this);

        if (patterCycle != null)
        {
            StopCoroutine(patterCycle);
        }

        patterCycle = StartCoroutine(CoroutinePatterCycle());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator CoroutinePatterCycle()
    {
        while(state != State.Death)
        {
            PatternCycle();
            yield return new WaitForSeconds(routineTime);
        }
    }

    private void PatternCycle()
    {
        if (state == State.Death)
        {
            return;
        }

        attackCooltime_Current += routineTime;
        if (PatternDelay != null)
        {
            if (PatternDelay())
                return;
        }
        if (state == State.Idle)
        {
            if(target != null)
            {
                state = State.Move;
            }
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
            StopMoveTarget();
            teleport.ActivePattern(Vector3.zero);
            state = State.Attack;
        }
        else if(state == State.Attack)
        {
            StopMoveTarget();
            if (attackCooltime_Current > attackCooltime_Max)
            {
                targetDirection = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z).normalized;
                enemy_Cross = Vector3.Cross((target.position - transform.position).normalized, new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));
                if (enemy_Cross.y > -0.3f && enemy_Cross.y < 0.2f && attackPatterns[2].CooltimeCheck())
                {
                    // 직선으로 4개
                    attackPatterns[2].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else if (attackPatterns[1].CooltimeCheck())
                {
                    // 옆으로 7개
                    angle = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
                    //enemyDirection = (transform.position - mapOriginPosition.position).normalized;
                    //if(math.abs(enemyDirection.x) > math.abs(enemyDirection.z))
                    //{
                    //    if(enemyDirection.x > 0)
                    //    {

                    //    }
                    //}
                    if (Random.value > 0.5)
                    {
                        angle += 90;
                    }
                    else
                    {
                        angle -= 90;
                    }
                    angle *= Mathf.Deg2Rad;
                    targetDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

                    attackPatterns[1].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else if (attackPatterns[0].CooltimeCheck())
                {
                    // 기본공격
                    attackPatterns[0].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else
                {
                    if (Vector3.Distance(transform.position, target.position) < runawayDistance)
                    {
                        state = State.Runaway;
                    }
                    else
                    {
                        state = State.Move;
                    }
                }

            }
            else
            {
                if(Vector3.Distance(transform.position, target.position) < runawayDistance)
                {
                    state = State.Runaway;
                }
                else
                {
                    state = State.Move;
                }
            }
        }

        if (attackCooltime_Current > attackCooltime_Max)
        {
            state = State.Attack;
        }

        if(state != State.Move)
        {
            setMovePosition = false;
        }
        else if(state != State.Runaway)
        {
            wall = false;
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
            if (Vector3.Distance(movementPosition, transform.position) < 0.5)
            {
                setMovePosition = false;
            }
            else
            {
                return;
            }
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

        if (teleport.CooltimeCheck())
        {
            print(Vector3.Distance(transform.position, target.position));

            if (Vector3.Distance(transform.position, target.position) < teleportDistance)
            {
                state = State.Teleport;
                return;
            }

        }

        enemyDistance = Vector3.Distance(mapOriginPosition.position, transform.position);
        if (enemyDistance < (mapRadius/4)*3 && !wall)
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
            enemy_Cross = Vector3.Cross((mapOriginPosition.position - transform.position).normalized, new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));

            if (enemy_Cross.y < -0.5)
            {
                angle += 80;
            }
            else if(enemy_Cross.y > 0.5)
            {
                angle -= 80;
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
