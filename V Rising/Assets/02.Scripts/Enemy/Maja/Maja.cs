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

    public float attackDelayTime;

    public float delayTime = 0;
    protected bool patterDelay;
    public float range;
    protected  abstract IEnumerator PatternDelayTime();
    protected abstract IEnumerator Coroutine_AttackPattern(Vector3 direction);
    protected abstract IEnumerator PatternCooltime();
}


public class Maja : Enemy
{
    public enum State
    {
        Idle,
        Move,
        Runaway,
        Teleport,
        Attack,
        Death,
    }

    public State state;

    public Maja_Minion minion_Prefab;

    public Transform mapOrigin;
    public List<Pattern> attackPatterns;
    public Pattern teleport;

    public float mapRadius = 10;
    public float runawayDistance = 5;
    public float teleportDistance = 2;

    private bool check_NormalAttck = true;
    private Vector3 movementPosition;
    private float enemyDistance;
    private bool distanceCheck = false;
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

    public List<Maja_Minion> maja_Minions = new List<Maja_Minion>();

    void Awake()
    {
        InitEnemy();
    }
    private void Update()
    {
        Rotate();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected new void InitEnemy()
    {
        base.InitEnemy();

        state = State.Idle;

        attackPatterns = new List<Pattern>();

        Pattern pattern = GetComponent<Maja_BasicAttackPattern>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_NormalSkillPattern1>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_MainSkillPattern1>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_MainSkillPattern2>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_MainSkillPattern3>();
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


    public void SpawnMinion(Vector3 position)
    {
        Maja_Minion minion = Instantiate(minion_Prefab);
        minion.transform.position = position;
        AddMinion(minion);
    }

    public void AddMinion(Maja_Minion minion)
    {
        maja_Minions.Add(minion);
        if(maja_Minions.Count > 6)
        {
            if (maja_Minions[0] != null)
            {
                maja_Minions[0].UpdateHP(-1000, false);
            }
        }
    }

    public void RemoveMinion(Maja_Minion minion)
    {
        if (maja_Minions.Contains(minion))
        {
            maja_Minions.Remove(minion);
        }
    }

    public Maja_Minion GetCloseMinion()
    {
        if (maja_Minions.Count == 0)
            return null;

        float min = float.MinValue;
        float check;
        int index = 0;
        
        for (int i = 0; i < maja_Minions.Count; i++)
        {
            check = Vector3.Distance(mapOrigin.position, maja_Minions[i].transform.position);
            if (min > check)
            {
                min = check;
                index = i;
            }
        }

        return maja_Minions[index];
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
        if (!alive)
        {
            state = State.Death;
        }

        if (state == State.Death)
        {
            return;
        }
        else if(target == null)
        {
            return;
        }

        attackCooltime_Current += routineTime;

        if (PatternDelay != null)
        {
            if (PatternDelay())
            {

                animator.SetBool("Walk", false);
                return;
            }
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
            animator.SetBool("Walk", true);
            Move(Vector3.zero);
        }
        else if (state ==State.Runaway)
        {
            animator.SetBool("Walk", true);
            Runaway();
        }
        else if(state == State.Teleport)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Teleport");
            StopMoveTarget();
            teleport.ActivePattern(Vector3.zero);
            state = State.Attack;
        }
        else if(state == State.Attack)
        {
            StopMoveTarget();
            if (attackCooltime_Current > attackCooltime_Max)
            {
                targetDirection = target.position- transform.position;
                targetDirection.y = 0;
                targetDirection = targetDirection.normalized;

                forward = new Vector3(target.position.x - model.position.x, model.position.y, target.position.z - model.position.z).normalized;
                enemy_Cross = Vector3.Cross((target.position - transform.position).normalized, new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));
                if (enemy_Cross.y > -0.3f && enemy_Cross.y < 0.2f && attackPatterns[1].CooltimeCheck())
                {
                    // 직선으로 4개
                    animator.SetTrigger("NormalSkillPattern");
                    attackPatterns[1].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else if (attackPatterns[2].CooltimeCheck())
                {
                    // 옆으로 7개
                    animator.SetTrigger("MainSkillPattern1");
                    attackPatterns[2].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else if (attackPatterns[4].CooltimeCheck())
                {
                    attackPatterns[4].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else if (attackPatterns[3].CooltimeCheck())
                {
                    attackPatterns[3].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else if (attackPatterns[0].CooltimeCheck())
                {
                    // 기본공격
                    animator.SetTrigger("AttackPattern1");
                    attackPatterns[0].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                else
                {
                    forward = Vector3.zero;
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

        // 쿨타임 체크 후 스킬을 쓸 수 있다면 사거리 만큼 이동
        if (attackCooltime_Current > attackCooltime_Max)
        {
            enemyDistance = Vector3.Distance(target.position, transform.position);
            bool check = false;
            for (int i = 0; i < attackPatterns.Count; i++)
            {
                if(attackPatterns[i].CooltimeCheck())
                {
                    if (enemyDistance <= attackPatterns[i].range)
                    {
                        state = State.Attack;
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;
                    }
                }
            }

            if (check)
            {
                state = State.Move;
                Move(target.position);
            }
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


    private void Move(Vector3 position)
    {
        if (position == Vector3.zero)
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


            enemyDirection = new Vector3(transform.position.x - mapOrigin.position.x, 0, transform.position.z - mapOrigin.position.z).normalized;
            angle = Mathf.Atan2(enemyDirection.z, enemyDirection.x) * Mathf.Rad2Deg;

            if (Random.value > 0.5)
            {
                angle += 70;
            }
            else
            {
                angle -= 70;
            }
            angle *= Mathf.Deg2Rad;
            movementPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (mapRadius - 1);
            movementPosition += mapOrigin.position;

            forward = new Vector3(movementPosition.x - model.position.x, 0, movementPosition.z - model.position.z).normalized;
            navMeshAgent.SetDestination(movementPosition);
            setMovePosition = true;
        }
        else
        {
            if(Vector3.Distance(mapOrigin.position, target.position) < mapRadius)
            {
                movementPosition = position;
                forward = new Vector3(movementPosition.x - model.position.x, 0, movementPosition.z - model.position.z).normalized;
                navMeshAgent.SetDestination(movementPosition);
                setMovePosition = true;
            }
        }

    }

    private void Runaway()
    {
        if (target == null)
            return;

        if (teleport.CooltimeCheck())
        {
            if (Vector3.Distance(transform.position, target.position) < teleportDistance)
            {
                state = State.Teleport;
                return;
            }

        }

        enemyDistance = Vector3.Distance(mapOrigin.position, transform.position);
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
            enemyDirection = new Vector3(transform.position.x - mapOrigin.position.x, 0 , transform.position.z - mapOrigin.position.z).normalized;
            targetDirection = new Vector3(target.position.x - mapOrigin.position.x, 0, target.position.z - mapOrigin.position.z).normalized;
            angle = Mathf.Atan2(enemyDirection.z, enemyDirection.x) * Mathf.Rad2Deg;
            // 이동 방향 구하기
            enemy_Cross = Vector3.Cross((mapOrigin.position - transform.position).normalized, new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));

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
                movementPosition += mapOrigin.position;
                navMeshAgent.SetDestination(movementPosition);
                return;
            }

            angle *= Mathf.Deg2Rad;
            movementPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (mapRadius - 1);
            movementPosition += mapOrigin.position;
        }
        forward = new Vector3(movementPosition.x - model.position.x, 0, movementPosition.z - model.position.z).normalized;
        navMeshAgent.SetDestination(movementPosition);
    }


}
