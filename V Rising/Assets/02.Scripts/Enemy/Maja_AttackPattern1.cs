using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_AttackPattern1 : MonoBehaviour
{
    private Transform player;

    public float coolTime = 5;
    public float coolTime_Current = 0;

    public float parentDistance = 10;
    public float childDistanece = 10;
    public float parentActiveTime = 3;
    public float childActiveTime = 3;

    public int childBulletCount = 8;
    
    public float parentDamage = 10;
    public float childDamage = 10;

    public bool secondDirection_Right;


    public Projectile parentProjectile_Prefab;
    public Projectile childProjectile_Prefab;
    private Projectile parentProjectile;
    private Projectile childProjectile;

    private List<Projectile> childProjectiles;

    public void ActivePattern(Vector3 firstDirection, bool secondDirection_Right)
    {
        parentProjectile = Instantiate(parentProjectile_Prefab);
        parentProjectile.transform.position = transform.position + (-firstDirection * 3);
        parentProjectile.transform.LookAt(transform.position + firstDirection);
        parentProjectile.InitAttack(parentDamage, true);
        this.secondDirection_Right = secondDirection_Right;
        parentProjectile.Fire(firstDirection, parentDistance, parentActiveTime);
    }

    private void ActiveChildProjectile(float  ti)
    {
        childProjectile = Instantiate(childProjectile_Prefab);
        childProjectile.transform.position = parentProjectile.transform.position;
        childProjectile.transform.rotation = parentProjectile.transform.rotation;
        if (secondDirection_Right)
        {
            childProjectile.transform.Rotate(0, 90, 0);
        }
        else
        {
            childProjectile.transform.Rotate(0, -90, 0);
        }

        childProjectile.InitAttack(parentDamage, true);
        childProjectiles.Add(childProjectile);
    }

    IEnumerator SpawnChileProjectilet()
    {
        for (int i = 0; i < childBulletCount; i++)
        {

        }
    }
}
