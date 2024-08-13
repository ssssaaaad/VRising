using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HP_Scan_Range : MonoBehaviour
{
    private Fblood Fblood;

    //public LayerMask layerMask;
    //public float radius;
    private bool canF = false;

    public List<Collider> Enemys = new List<Collider>();
    public Collider closeEnemy;

    //private Collider[] colliders;

    private void Start()
    {
        Fblood = GetComponentInParent<Fblood>();

        //colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
    }

    private void Update()
    {
        if (Enemys.Count > 0)
        {
            //    for (int i = Enemys.Count - 1; i >= 0; i--)
            //    {
            //        print(i);
            //        // 리스트 안에 있는 적이 죽었을 경우 리스트에서 제거
            //        if (!Enemys[i].GetComponentInParent<Enemy>().alive)
            //        {
            //            if (Enemys[i] == closeEnemy)
            //                closeEnemy = null;
            //            Enemys.Remove(Enemys[i]);
            //        }
            //        else if (closeEnemy == null)
            //        {
            //            closeEnemy = Enemys[i];
            //        }
            //        else
            //        {
            //            if (Vector3.Distance(transform.position, closeEnemy.transform.position)
            //                > Vector3.Distance(transform.position, Enemys[i].transform.position))
            //            {
            //                closeEnemy = Enemys[i];
            //            }
            //        }
            //    }
            closeEnemy = Enemys[Enemys.Count - 1];
        }
        //else
        //{
        //    closeEnemy = null;
        //}



        if (closeEnemy != null)
            canF = true;
        else
            canF = false;

    }

    private void OnTriggerStay(Collider other)
    {
        // 리스트에 저장
        if (other.GetComponentInParent<Enemy>().Drain())
        {
            InGameUIController.instance.BloodSkill_lcon_true(other.transform);
            closeEnemy = other;
        }
            //Enemys.Add(other);
    }


    private void OnTriggerExit(Collider other)
    {
        if (Enemys.Contains(other))
        {
            InGameUIController.instance.BloodSkill_lcon_false();
            Enemys.Remove(other);
            closeEnemy = null;
        }
    }

    public bool CanF()
    {
        return canF;
    }

}
