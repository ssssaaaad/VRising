using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Maja : Enemy
{
    public Transform mapOriginPosition;

    public Transform normalAttack;
    private bool check_NormalAttck = true;
    private Vector3 movementPosition;
    private float mapRadius = 10;
    private float enemyDistance;

    void Awake()
    {
        InitEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        print(1);
        Runaway();
    }

    protected new void InitEnemy()
    {
        base.InitEnemy();

    }

    private void NormalAttack()
    {

        check_NormalAttck = false;
    }

    private void Runaway()
    {
        if(target == null)
            return;
        enemyDistance = Vector3.Distance(mapOriginPosition.position, transform.position);
        if (enemyDistance < mapRadius)
        {
            movementPosition = transform.position + (transform.position - target.position).normalized * (mapRadius - enemyDistance);
        }
        //else
        //{
        //    Mathf.Atan2(transform.z, transform.x);
        //}

        navMeshAgent.SetDestination(movementPosition);
    }
}
