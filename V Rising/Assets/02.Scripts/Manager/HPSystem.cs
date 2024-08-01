using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    public Transform player;

    public float maxHp = 100f;
    public float currHP;
    public Image heathBar;
    


    void Start()
    {
        currHP = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}