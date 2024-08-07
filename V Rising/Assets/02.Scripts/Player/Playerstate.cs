using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playerstate : MonoBehaviour
{
    public float hp_Max = 500;
    public float hp_Current;
    public float power = 40;
    
    void Start()
    {
        hp_Current = hp_Max;
    }

    void Update()
    {
        
    }


    public void UpdateHP(float dmg)
    {
        if (hp_Current > 0)
            hp_Current -= dmg;
    }
}
