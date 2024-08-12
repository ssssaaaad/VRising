using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Enemy obj1;
    public bool attack = false;

    private void Update()
    {
        if (attack)
        {
            attack = false;
            obj1.UpdateHP(-10, transform);
        }
    }

}
