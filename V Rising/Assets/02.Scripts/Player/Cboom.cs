using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cboom : MonoBehaviour
{
    private Cskill Cskill;
    private Playerstate PS;

    void Start()
    {
        Cskill = FindObjectOfType<Cskill>();
        PS = FindObjectOfType<Playerstate>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("폭발");
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Cskill.Damage(other, Cskill.Cdmg);
        }
        
    }

}
