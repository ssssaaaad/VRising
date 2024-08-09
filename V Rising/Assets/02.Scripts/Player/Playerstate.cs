using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playerstate : MonoBehaviour
{
    private PlayerManager PM;
    private Cskill Cskill;

    public float hp_Max = 500;
    public float hp_Current;
    public float power = 40;
    
    void Start()
    {
        hp_Current = hp_Max;
        PM = GetComponent<PlayerManager>();
        Cskill = GetComponent<Cskill>();
    }

    void Update()
    {
        
    }

    public void UpdateHP(float dmg)
    {
        print(1);
        if (PM.cskilling)
        {
            Cskill.Cdmg = dmg;
            Cskill.counter = true;
        }
        else if (hp_Current > 0)
            hp_Current -= dmg;
    }
}
