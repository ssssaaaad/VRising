using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Maja_Minion : Enemy
{
    public Maja maja;
    public float targetOriginDistance;
    public float targetEnemyDistance;
    public float speed;
    public float attackRange;
    private float mapRadius;
    private Transform mapOrigin;
    private Vector3 randomDirection;
    private Vector3 moveDirection;
    private Vector3 moveposition;

    public float tarceTime_Max = 5;
    private float tarceTime = 0;
    private float randomDistance;

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
        mapRadius = maja.mapRadius;
        mapOrigin = maja.mapOriginPosition;
        ResetRandomDirection();
    }

    private void ResetRandomDirection()
    {
        randomDirection = UnityEngine.Random.insideUnitCircle;
        randomDistance = UnityEngine.Random.Range(5, 10);
        tarceTime = 0;
    }

    private void SetMovePosition_Target()
    {
        if(target == null)
        {
            return;
        }

        targetEnemyDistance = Vector3.Distance(target.position, transform.position);
        targetOriginDistance = Vector3.Distance(mapOrigin.position, target.position);
        moveDirection = (target.position - mapOrigin.position);
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;
        moveposition = moveDirection * targetOriginDistance;
        moveposition += mapOrigin.position;

        tarceTime += Time.deltaTime; 
        if (tarceTime >= tarceTime_Max)
        {
            ResetRandomDirection();
        }
        moveposition += randomDirection * Mathf.Lerp(randomDistance, 0, tarceTime/tarceTime_Max);


        if (Vector3.Distance(mapOrigin.position, moveposition) > mapRadius)
        {
            moveposition = (moveposition - mapOrigin.position).normalized;
            moveposition *= mapRadius + Random.Range(-3,0);
        }
        MovePosition(moveposition);

        if (targetEnemyDistance <= attackRange)
        {
            state = State.Attack;
        }
    }

    private void Attack()
    {
        
    }
}
