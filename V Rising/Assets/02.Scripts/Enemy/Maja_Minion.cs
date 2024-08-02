using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_Minion : Enemy
{
    public Transform _target;
    public Transform _p1, _p2, _p3;
    public float _distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void SetMovePosition_BezierCurves()
    //{
    //    float time = 0f;
    //    if(target == null)
    //    _distance = Vector3.Distance(target.position, )

       
    //    if (time > 1f)
    //    {
    //        time = 0f;
    //    }

    //    Vector3 p4 = Vector3.Lerp(_p1.position, _p2.position, time);
    //    Vector3 p5 = Vector3.Lerp(_p2.position, _p3.position, time);
    //    _target.position = Vector3.Lerp(p4, p5, time);

    //    time += Time.deltaTime / duration;

        
    //}
}
