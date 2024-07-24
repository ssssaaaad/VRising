using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Maja : Enemy
{
    public Transform mapOriginPosition;

    public Transform normalAttack;
    private bool check_NormalAttck = true;

    void Awake()
    {
        InitEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected new void InitEnemy()
    {
        base.InitEnemy();

    }

    private void NormalAttack()
    {

        check_NormalAttck = false;
    }
}
