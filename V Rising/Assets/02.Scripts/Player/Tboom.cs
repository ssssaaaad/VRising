using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tboom : MonoBehaviour
{
    private Tskill Tskill;
    private Playerstate PS;

    void Start()
    {
        Tskill = FindObjectOfType<Tskill>();
        PS = FindObjectOfType<Playerstate>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Tskill.Damage(other, Tskill.Tdmg);
            PS.UpdateHP(PS.power * Tskill.Tdmg * 0.3f);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3);
    }

}
