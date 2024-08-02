using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Maja_Minion : Enemy
{
    public Transform _p1, _p2;
    public Transform _p4, _p5;
    public float targetOriginDistance;
    public float targetEnemyDistance;
    public float speed;
    public float attackRange;
    public Transform mapOrigin;
    // Start is called before the first frame update
    void Awake()
    {
        InitEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        SetMovePosition_BezierCurves();
    }
    protected new void InitEnemy()
    {
        base.InitEnemy();
        traceTime = traceMaxTime;
    }
    public float traceMaxTime = 50;
    public float traceTime;
    private void SetMovePosition_BezierCurves()
    {
        //if (target != null)
        //    targetDistance = Vector3.Distance(target.position, transform.position);
        //else
        //{
        //    return;
        //}
        //time += Time.deltaTime * speed;

        //Vector3 p4 = Vector3.Lerp(_p1.position, _p2.position, time);
        //Vector3 p5 = Vector3.Lerp(_p2.position, target.position, time);

        //_p4.position = p4;
        //_p5.position = p5;

        //MovePosition(Vector3.Lerp(p4, p5, time));

        //if(targetDistance < attackRange)
        //{
        //    _p1.position = transform.position;
        //    time = 0;
        //}

        if(target == null)
        {
            return;
        }

        targetEnemyDistance = Vector3.Distance(target.position, transform.position);
        targetOriginDistance = Vector3.Distance(mapOrigin.position, target.position);
        Vector3 d = (target.position - mapOrigin.position).normalized;
        float angle = Vector3.Angle(Vector3.forward, d);
        angle += UnityEngine.Random.Range(-traceTime, traceTime) * Mathf.Deg2Rad;
        d = new Vector3(math.cos(angle), math.sin(angle));
        d += mapOrigin.position;
        d *= targetOriginDistance;
        MovePosition(d);

        if(targetEnemyDistance <= attackRange)
        {
            print("a");
            traceTime = traceMaxTime;
        }
    }
}
