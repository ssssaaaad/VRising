using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystem : MonoBehaviour
{
    public Transform player;
    
    public float maxHp;
    public float currenthp;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + new Vector3(0, 0, 0);
       
    }
}