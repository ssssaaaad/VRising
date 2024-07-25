using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public Transform target;

    public float hp_Max;
    public float hp_Current;

    protected bool alive = false;

    protected void InitEnemy()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        alive = true;
    }

    private void Start()
    {
        InitEnemy();
    }

    /// <summary>
    /// 음수는 데미지, 양수는 회복
    /// </summary>
    /// <param name="dmg"></param>
    public void UpdateHP(float dmg)
    {
        if (!alive)
            return;
        hp_Current = Mathf.Clamp(hp_Current + dmg, 0, hp_Max);
        
        if(hp_Current <= 0)
        {
            StopMoveTarget();
            alive = false;
        }
    }

    protected void MoveTarget(Transform target)
    {
        this.target = target;
        navMeshAgent.SetDestination(target.position);
    }

    protected void StopMoveTarget()
    {
        navMeshAgent.ResetPath();
        target = null;
    }
    
}
