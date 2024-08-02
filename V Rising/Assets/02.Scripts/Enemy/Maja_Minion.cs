using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Maja_Minion : Enemy
{
    public Maja maja;
    public float targetOriginDistance;
    public float targetEnemyDistance;
    public float speed;
    public float traceRange = 3;
    public float attackRange;
    private Vector3 moveDirection;
    private Vector3 moveposition;

    public float randomDistance;
    public float randomDirection;

    public float traceTime_Max = 3;
    public float traceTime = 0;

    // Start is called before the first frame update
    void Awake()
    {
        InitEnemy(maja);
    }

    // Update is called once per frame
    void Update()
    {
        SetMovePosition_Target();
    }
    protected new void InitEnemy(Maja maja)
    {
        base.InitEnemy();
        this.maja = maja;
        ResetRandomDirection();
    }

    private void ResetRandomDirection()
    {
        //randomDirection = UnityEngine.Random.insideUnitCircle;
        randomDirection = Random.value > 0.5 ? -1 : 1;
        randomDistance = Random.Range(maja.mapRadius/2, maja.mapRadius-1);
    }

    private void SetMovePosition_Target()
    {
        if(target == null)
        {
            return;
        }

        targetEnemyDistance = Vector3.Distance(target.position, transform.position);
        if (targetEnemyDistance > traceRange)
        {
            moveDirection = (transform.position - maja.mapOrigin.position);
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;
            float angle = Mathf.Atan2(moveDirection.z, moveDirection.x);
            angle += (70 * randomDirection) * Mathf.Deg2Rad;
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
            aa.transform.position = moveposition;
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

    public Transform aa;
    private void Attack()
    {
        
    }
}
