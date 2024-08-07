using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{

    public bool alive { get; private set; } = true;

    #region NavMesh
    public NavMeshAgent navMeshAgent;
    public Transform target;

    #endregion

    public Transform model;

    #region status
    public float hp_Max;
    public float hp_Current;
    #endregion

    #region rotate
    public float rotateSpeed = 130;
    protected Vector3 forward = Vector3.zero;
    #endregion

    #region Animation
    public Animator animator;
    #endregion


    public void InitEnemy()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        alive = true;
    }


    /// <summary>
    /// 음수는 데미지, 양수는 회복
    /// </summary>
    /// <param name="dmg"></param>
    public void UpdateHP(float dmg, bool uiActvie = true)
    {
        if (!alive)
            return;
        hp_Current = Mathf.Clamp(hp_Current + dmg, 0, hp_Max);

        if (hp_Current <= 0)
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
                forward = Vector3.zero;
            }
        }
    }
}
