using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{

    public bool alive { get; private set; } = true;
    public bool drain = false;
    public bool canDrain = true;
    #region NavMesh
    public NavMeshAgent navMeshAgent;
    public Transform target;

    #endregion

    public Transform model;
    protected Transform origin;

    #region status
    public float hp_Max;
    public float hp_Current;
    #endregion

    #region rotate
    public float rotateSpeed = 130;
    public Vector3 forward = Vector3.zero;
    #endregion

    #region Animation
    public Animator animator;
    #endregion

    public Transform effectPosition;

    public void InitEnemy()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        hp_Current = hp_Max;
        alive = true;
    }

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
    }


    public bool Drain()
    {
        if (drain || !canDrain)
            return false;
        if (hp_Current / hp_Max > 0.1f)
            return false;

        animator.speed = 0;
        drain = true;
        return true;
    }
    public void FinishDrain()
    {
        animator.speed = 1;
    }


    /// <summary>
    /// 음수는 데미지, 양수는 회복
    /// </summary>
    /// <param name="dmg"></param>
    public void UpdateHP(float dmg, Transform target, bool uiActvie = true)
    {
        print(1);
        if (!alive)
            return;
        hp_Current = Mathf.Clamp(hp_Current + dmg, 0, hp_Max);

        if(target != null)
        {
            this.target = target;
        }

        if (hp_Current <= 0)
        {
            StopMoveTarget();
            alive = false;
        }
        else
        {
            SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.BossHit, transform, Vector3.zero);
        }

    }
    public void SetTarget(Transform target)
    {
        if (target != null)
        {
            this.target = target;
        }
    }
    protected void MoveTarget(Transform target)
    {
        this.target = target;
        navMeshAgent.SetDestination(target.position);
    }
    protected void MovePosition(Vector3 position)
    {
        //this.target = null;
        navMeshAgent.SetDestination(position);
    }

    public void StopMoveTarget()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.velocity = Vector3.zero;
    }

    protected void Rotate()
    {
        if(forward == Vector3.zero)
            return;

        float checker = 0;
        float angle;
        angle = Vector3.Angle(model.forward, forward);
        Vector3 cross = Vector3.Cross(model.forward, forward);
        if (cross.y > 0.1)
            model.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        else if (cross.y < -0.1)
            model.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0));
        else
        {
            if (angle > 90)
            {
                model.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0));
            }
            else
            {
                //forward = Vector3.zero;
            }
        }

    }
}
