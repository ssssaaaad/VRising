using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform obj1;

    public Vector3 forward;

    private void Update()
    {
        Rotate();
    }
    protected void Rotate()
    {
        float angle;
        forward = obj1.position - transform.position;
        angle = Vector3.Angle(transform.forward, forward);
        Vector3 cross = Vector3.Cross(transform.forward, forward);

        if (cross.y > 0.2)
            transform.Rotate(new Vector3(0, Time.deltaTime * 0.5f, 0));
        else if (cross.y < -0.2)
            transform.Rotate(new Vector3(0, Time.deltaTime * 0.5f, 0));
    }

}
