using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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

    #region NavMesh
    public NavMeshAgent navMeshAgent;
    public Transform target;

    #endregion

    #region status
    public float hp_Max;
    public float hp_Current;
    public State state;
    #endregion

    #region Animation
    public Animator animator;
    #endregion


    protected void InitEnemy()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        state = State.Idle;
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
        if (state != State.Death)
            return;
        hp_Current = Mathf.Clamp(hp_Current + dmg, 0, hp_Max);
        
        if(hp_Current <= 0)
        {
            StopMoveTarget();
            state = State.Death;
        }
    }

    protected void MoveTarget(Transform target)
    {
        this.target = target;
        navMeshAgent.SetDestination(target.position);
    }
    protected void MovePosition(Vector3 position)
    {
        this.target = null;
        navMeshAgent.SetDestination(position);
    }

    public void StopMoveTarget()
    {
        navMeshAgent.ResetPath();
    }
    
}
