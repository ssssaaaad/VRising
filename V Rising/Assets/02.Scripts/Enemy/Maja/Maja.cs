using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public delegate bool PatternDelay();

public abstract class Pattern : MonoBehaviour
{
    public abstract void InitPattern(Maja maja);
    public abstract void ActivePattern(Vector3 direction);

    public abstract void SetDamage(float dmg);
    public abstract bool CooltimeCheck();

    protected abstract bool GetPatternDelay();

    public float attackDelayTime;

    public float delayTime = 0;
    protected bool patterDelay;
    public float range;
    protected  abstract IEnumerator PatternDelayTime();
    protected abstract IEnumerator Coroutine_AttackPattern(Vector3 direction);
    protected abstract IEnumerator PatternCooltime();

    protected abstract IEnumerator VFXAcitve(VFX vfx);



    public VFX[] vfxList = new VFX[] { };


    public float damage = 50;
    public float damage_Min = 50;
    public float damage_Max = 75;
}

[System.Serializable]
public class VFX
{
    public GameObject vfxObject;
    public float startTime;
    public float operatingTime;
    public bool localPosition = true;
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

    public float attackCooltime_Max = 3;
    private float attackCooltime_Current = 2;
    private float routineTime = 0.2f;

    public PatternDelay PatternDelay;
    private Coroutine patterCycle;

    public List<Maja_Minion> maja_Minions = new List<Maja_Minion>();

    public List<Pattern> phase1_Start = new List<Pattern>();
    public List<Pattern> phase1_Loop = new List<Pattern>();
    public List<Pattern> phase2_Start = new List<Pattern>();
    public List<Pattern> phase2_Loop = new List<Pattern>();
    public List<Pattern> phase3_Start = new List<Pattern>();
    public List<Pattern> phase3_Loop = new List<Pattern>();
    public List<Pattern> phase4_Start = new List<Pattern>();
    public List<Pattern> phase4_Loop = new List<Pattern>();

    

    private List<Pattern> phase_Start;
    private List<Pattern> phase_Loop;
    private bool startPatternEnd = false;
    private int index = 0;

    public int phase = 0;

    public SFXAudioSource talkSound = null;

    private bool[] phases = new bool[] { false, false, false, false };

    public GameObject[] particles;

    IEnumerator ResetParticle()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].SetActive(false);
        }
    }

    void Awake()
    {
        InitEnemy();
    }


    private void Update()
    {
        Rotate();

        if (Input.GetKeyDown(KeyCode.P))
        {
            UpdateHP(-hp_Max*0.1f, null);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected new void InitEnemy()
    {
        base.InitEnemy();

        InitPattern();
        state = State.Idle;

        if (patterCycle != null)
        {
            StopCoroutine(patterCycle);
        }

        patterCycle = StartCoroutine(CoroutinePatterCycle());
        animator.SetBool("IsAlive", alive);
        drainFinishEvent += () =>
        {
            animator.SetTrigger("Death");
        };
        StartCoroutine(ResetParticle());
    }


    private void InitPattern()
    {
        attackPatterns = new List<Pattern>();

        Pattern pattern = GetComponent<Maja_BasicAttackPattern>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_NormalSkillPattern1>();
        attackPatterns.Add(pattern);
        pattern = GetComponent<Maja_NormalSkillPattern2>();
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
            attackPatterns[i].SetDamage(attackPatterns[i].damage_Min);
        }

        teleport = GetComponent<Maja_Teleport>();
        teleport.InitPattern(this);

        phase1_Start.Add(attackPatterns[0]);
        phase1_Start.Add(attackPatterns[1]);
        phase1_Start.Add(attackPatterns[2]);
        phase1_Start.Add(attackPatterns[3]);
        phase1_Start.Add(attackPatterns[4]);
        phase1_Start.Add(attackPatterns[5]);

        phase1_Loop.Add(attackPatterns[1]);
        phase1_Loop.Add(attackPatterns[0]);
        phase1_Loop.Add(attackPatterns[1]);
        phase1_Loop.Add(attackPatterns[3]);
        phase1_Loop.Add(attackPatterns[0]);
        phase1_Loop.Add(attackPatterns[1]);
        phase1_Loop.Add(attackPatterns[0]);
        phase1_Loop.Add(attackPatterns[1]);
        phase1_Loop.Add(attackPatterns[3]);


        phase2_Start.Add(attackPatterns[0]);
        phase2_Start.Add(attackPatterns[1]);
        phase2_Start.Add(attackPatterns[4]);

        phase2_Loop.Add(attackPatterns[0]);
        phase2_Loop.Add(attackPatterns[1]);
        phase2_Loop.Add(attackPatterns[0]);
        phase2_Loop.Add(attackPatterns[1]);
        phase2_Loop.Add(attackPatterns[4]);
        phase2_Loop.Add(attackPatterns[0]);
        phase2_Loop.Add(attackPatterns[1]);
        phase2_Loop.Add(attackPatterns[0]);
        phase2_Loop.Add(attackPatterns[1]);
        phase2_Loop.Add(attackPatterns[3]);


        phase3_Start.Add(attackPatterns[0]);
        phase3_Start.Add(attackPatterns[1]);
        phase3_Start.Add(attackPatterns[1]);
        phase3_Start.Add(attackPatterns[4]);
        phase3_Start.Add(attackPatterns[5]);

        phase3_Loop.Add(attackPatterns[1]);
        phase3_Loop.Add(attackPatterns[3]);
        phase3_Loop.Add(attackPatterns[0]);
        phase3_Loop.Add(attackPatterns[4]);
        phase3_Loop.Add(attackPatterns[2]);
        phase3_Loop.Add(attackPatterns[3]);
        phase3_Loop.Add(attackPatterns[1]);
        phase3_Loop.Add(attackPatterns[1]);
        phase3_Loop.Add(attackPatterns[0]);
        phase3_Loop.Add(attackPatterns[4]);


        phase4_Start.Add(attackPatterns[5]);
        phase4_Start.Add(attackPatterns[4]);
        phase4_Start.Add(attackPatterns[3]);
        phase4_Start.Add(attackPatterns[4]);
        phase4_Start.Add(attackPatterns[0]);
        phase4_Start.Add(attackPatterns[1]);
        phase4_Start.Add(attackPatterns[3]);
        phase4_Start.Add(attackPatterns[4]);


        phase4_Loop.Add(attackPatterns[5]);
        phase4_Loop.Add(attackPatterns[1]);
        phase4_Loop.Add(attackPatterns[3]);
        phase4_Loop.Add(attackPatterns[4]);
        phase4_Loop.Add(attackPatterns[0]);
        phase4_Loop.Add(attackPatterns[1]);
        phase4_Loop.Add(attackPatterns[3]);
        phase4_Loop.Add(attackPatterns[4]);
        phase4_Loop.Add(attackPatterns[2]);
        phase4_Loop.Add(attackPatterns[0]);
        phase4_Loop.Add(attackPatterns[5]);
    }
    
    private void CheckHP()
    {
        if (hp_Current / hp_Max > 0.7f)
        {
            if (phase_Loop == phase1_Loop || phases[0])
                return;
            phases[0] = true;
            attackCooltime_Max = 3;
            startPatternEnd = false;
            index = 0;
            phase_Start = phase1_Start;
            phase_Loop = phase1_Loop;
        }
        else if (hp_Current / hp_Max > 0.5f)
        {
            if (phase_Loop == phase2_Loop || phases[1])
                return;
            phase++;
            phases[1] = true;
            startPatternEnd = false;
            index = 0;
            phase_Start = phase2_Start;
            phase_Loop = phase2_Loop;

            for (int i = 0; i < attackPatterns.Count; i++)
            {
                float dmg = attackPatterns[i].damage_Max - attackPatterns[i].damage_Min;
                dmg = attackPatterns[i].damage + dmg / 3;
                attackPatterns[i].SetDamage(dmg);
            }

            for (int i = 0; i < maja_Minions.Count; i++)
            {
                float dmg = (maja_Minions[i].damage_Max - maja_Minions[i].damage_Min);
                dmg = maja_Minions[i].damage + (dmg / 3 * phase);
                maja_Minions[i].SetDamage(dmg);
            }
            
        }
        else if (hp_Current / hp_Max > 0.3f)
        {
            if (phase_Loop == phase3_Loop || phases[2])
                return;
            phase++;
            phases[2] = true;
            attackCooltime_Max = 2;
            startPatternEnd = false;
            index = 0;
            phase_Start = phase3_Start;
            phase_Loop = phase3_Loop; 
            
            for (int i = 0; i < attackPatterns.Count; i++)
            {
                float dmg = attackPatterns[i].damage_Max - attackPatterns[i].damage_Min;
                dmg = attackPatterns[i].damage + dmg / 3;
                attackPatterns[i].SetDamage(dmg);
            }

            for (int i = 0; i < maja_Minions.Count; i++)
            {
                float dmg = (maja_Minions[i].damage_Max - maja_Minions[i].damage_Min);
                dmg = maja_Minions[i].damage + (dmg / 3 * phase);
                maja_Minions[i].SetDamage(dmg);
            }
        }
        else
        {
            if (phase_Loop == phase4_Loop || phases[3])
                return;
            phase++;
            phases[3] = true;
            startPatternEnd = false;
            index = 0;
            phase_Start = phase4_Start;
            phase_Loop = phase4_Loop;

            for (int i = 0; i < attackPatterns.Count; i++)
            {
                float dmg = attackPatterns[i].damage_Max - attackPatterns[i].damage_Min;
                dmg = attackPatterns[i].damage + dmg / 3;
                attackPatterns[i].SetDamage(dmg);
            }
            for (int i = 0; i < maja_Minions.Count; i++)
            {
                float dmg = (maja_Minions[i].damage_Max - maja_Minions[i].damage_Min);
                dmg = maja_Minions[i].damage + (dmg / 3 * phase);
                maja_Minions[i].SetDamage(dmg);
            }
        }
    }

    public void SpawnMinion(Vector3 position)
    {
        if (hp_Current == 0)
            return;

        if (Vector3.Distance(origin.position, position) > mapRadius)
        {
            position += (origin.position - position).normalized * (Vector3.Distance(origin.position, position) - mapRadius);
        }
        Maja_Minion minion = Instantiate(minion_Prefab);
        minion.transform.position = position;

        float dmg = (minion.damage_Max - minion.damage_Min);
        dmg = minion.damage + (dmg / 3 * phase);

        minion.InitEnemy(this, dmg);
    }

    public void AddMinion(Maja_Minion minion)
    {
        maja_Minions.Add(minion);
        if(maja_Minions.Count > 6)
        {
            if (maja_Minions[0] != null)
            {
                maja_Minions[0].UpdateHP(-1000, null,false);
                maja_Minions.RemoveAt(0);
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
            if (maja_Minions[i] == null)
            {
                maja_Minions.RemoveAt(i);
                if(maja_Minions.Count == 0)
                {
                    return null;
                }
            }
            check = Vector3.Distance(origin.position, maja_Minions[i].transform.position);
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

        if (state == State.Death)
        {
            if (!drain)
            {
                if(talkSound != null)
                {
                    if (!talkSound.audioSource.isPlaying)
                    { 
                        talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Die, transform, Vector3.zero);
                    }
                }
                else
                {
                    talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Die, transform, Vector3.zero);
                }
            }
            else
            {
                alive = false;
                if (talkSound != null)
                {
                    talkSound.StopSound();
                    talkSound = null;
                }
            }
            return;
        }
        else if(target == null && hp_Current > 0)
        {
            return;
        }
        else if (hp_Current == 0)
        {
            state = State.Death;
            animator.SetBool("IsAlive", false);
            animator.SetTrigger("Groggy");
            SoundManager.instance.FadeOut_BGM();
            canDrain = true;
            for (int i = 0; i < maja_Minions.Count; i++)
            {
                maja_Minions[i].UpdateHP(-10000, null, false);
            }

            if (talkSound != null)
            {
                talkSound.StopSound();
            }
            talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Die, transform, Vector3.zero);
        }

        CheckHP();

        if (PatternDelay != null)
        {
            if (PatternDelay())
            {
                animator.SetBool("Walk", false);
                return;
            }
        }

        attackCooltime_Current += routineTime;

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
            state = State.Idle;
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

                if(talkSound != null)
                {
                    if (!talkSound.audioSource.isPlaying)
                    {
                        talkSound = null;
                    }
                }

                if (!startPatternEnd)
                {
                    if (index >= phase_Start.Count)
                    {
                        index = 0;
                        startPatternEnd = true;
                    } 

                    phase_Start[index++].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;

                }
                else
                {
                    if (index >= phase_Loop.Count)
                    {
                        index = 0;
                    }

                    phase_Loop[index++].ActivePattern(targetDirection);
                    attackCooltime_Current = 0;
                }
                //if (enemy_Cross.y > -0.3f && enemy_Cross.y < 0.2f && attackPatterns[1].CooltimeCheck())
                //{
                //    // 직선으로 4개
                //    attackPatterns[1].ActivePattern(targetDirection);
                //    attackCooltime_Current = 0;
                //}
                //else if (attackPatterns[3].CooltimeCheck())
                //{
                //    // 옆으로 7개
                //    animator.SetTrigger("MainSkillPattern1");
                //    attackPatterns[3].ActivePattern(targetDirection);
                //    attackCooltime_Current = 0;
                //}
                //else if (attackPatterns[5].CooltimeCheck())
                //{
                //    attackPatterns[5].ActivePattern(targetDirection);
                //    attackCooltime_Current = 0;
                //}
                //else if (attackPatterns[4].CooltimeCheck())
                //{
                //    attackPatterns[4].ActivePattern(targetDirection);
                //    attackCooltime_Current = 0;
                //}
                //else if (attackPatterns[0].CooltimeCheck())
                //{
                //    print("기본");
                //    // 기본공격
                //    animator.SetTrigger("AttackPattern1");
                //    attackPatterns[0].ActivePattern(targetDirection);
                //    attackCooltime_Current = 0;
                //}
                //else if (attackPatterns[2].CooltimeCheck())
                //{
                //    print("기본");
                //    // 기본공격
                //    animator.SetTrigger("AttackPattern1");
                //    attackPatterns[2].ActivePattern(targetDirection);
                //    attackCooltime_Current = 0;
                //}
                //else
                //        {
                //    forward = Vector3.zero;
                //    if (Vector3.Distance(transform.position, target.position) < runawayDistance)
                //    {
                //        state = State.Runaway;
                //    }
                //    else
                //    {
                //        state = State.Move;
                //    }
                //}

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
                if (attackPatterns[i].CooltimeCheck())
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

        if (state != State.Move)
        {
            setMovePosition = false;
        }
        else if(state != State.Runaway)
        {
            wall = false;
        }
    }
    IEnumerator Scene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(3);
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


            enemyDirection = new Vector3(transform.position.x - origin.position.x, 0, transform.position.z - origin.position.z).normalized;
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
            movementPosition += origin.position;

            forward = new Vector3(movementPosition.x - model.position.x, 0, movementPosition.z - model.position.z).normalized;
            navMeshAgent.SetDestination(movementPosition);
            setMovePosition = true;
        }
        else
        {
            if(Vector3.Distance(origin.position, target.position) < mapRadius)
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

        enemyDistance = Vector3.Distance(origin.position, transform.position);
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
            enemyDirection = new Vector3(transform.position.x - origin.position.x, 0 , transform.position.z - origin.position.z).normalized;
            targetDirection = new Vector3(target.position.x - origin.position.x, 0, target.position.z - origin.position.z).normalized;
            angle = Mathf.Atan2(enemyDirection.z, enemyDirection.x) * Mathf.Rad2Deg;
            // 이동 방향 구하기
            enemy_Cross = Vector3.Cross((origin.position - transform.position).normalized, new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));

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
                movementPosition += origin.position;
                navMeshAgent.SetDestination(movementPosition);
                return;
            }

            angle *= Mathf.Deg2Rad;
            movementPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (mapRadius - 1);
            movementPosition += origin.position;
        }
        forward = new Vector3(movementPosition.x - model.position.x, 0, movementPosition.z - model.position.z).normalized;
        navMeshAgent.SetDestination(movementPosition);
    }


}
