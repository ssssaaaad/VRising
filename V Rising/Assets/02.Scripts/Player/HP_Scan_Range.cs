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
        if(Enemys != null)
        {
            for(int i = Enemys.Count - 1; i >= 0; i --)
            {
                // 리스트 안에 있는 적이 죽었을 경우 리스트에서 제거
                //if (!Enemys[i].GetComponentInParent<Enemy>().alive)
                //{
                //    Enemys.Remove(Enemys[i]);
                //}
                //else
                if (closeEnemy == null)
                {
                    closeEnemy = Enemys[i];
                }   
                else
                {
                    if (Vector3.Distance(transform.position, closeEnemy.transform.position)
                        > Vector3.Distance(transform.position, Enemys[i].transform.position))
                    {
                        closeEnemy = Enemys[i];
                    }
                }
            }
            canF = true;
        }
        else
        {
            canF = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // HP가 10퍼센트 이하라면
        // 리스트에 저장
        Enemys.Add(other);
    }


    private void OnTriggerExit(Collider other)
    {
        if(Enemys.Contains(other))
            Enemys.Remove(other);
    }

    public bool CanF()
    {
        return canF;
    }

}
