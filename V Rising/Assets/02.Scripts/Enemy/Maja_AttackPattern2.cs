using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_AttackPattern2 : MonoBehaviour
{
    public Maja maja;
    private Transform player;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float startPosition = 2;

    public float parentDistance = 10;
    public float childDistanece = 10;
    public float parentActiveTime = 3;
    public float childActiveTime = 3;

    public int childBulletCount = 8;
    
    public float parentDamage = 10;
    public float childDamage = 10;

    public bool secondDirection_Right;

    private Vector3 lookAtDirection;

    public Projectile parentProjectile_Prefab;
    public Projectile childProjectile_Prefab;
    private Projectile parentProjectile;
    private Projectile childProjectile;

    private List<Projectile> childProjectiles = new List<Projectile>();

    public bool start = false;
    public bool dir = false;

    private void Awake()
    {
        maja = GetComponent<Maja>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void ActivePattern(Vector3 direction)
    {
        if (!readyToStart)
        {
            return;
        }
        readyToStart = false;
        childProjectiles.Clear();

        parentProjectile = Instantiate(parentProjectile_Prefab);
        parentProjectile.transform.position = transform.position + (-direction * startPosition) + Vector3.up;
        lookAtDirection = (transform.position + direction);
        lookAtDirection.y = parentProjectile.transform.position.y;
        parentProjectile.transform.LookAt(lookAtDirection);
        parentProjectile.InitAttack(parentDamage, true, false);

        if(maja.target != null)
        {
            Vector3 enemy_Cross = Vector3.Cross(parentProjectile.transform.forward, (maja.target.position - parentProjectile.transform.position));

            if (enemy_Cross.y > 0)
            {
                this.secondDirection_Right = true;
            }
            else
            {
                this.secondDirection_Right = false;
            }
        }
        else
        {

        }
        parentProjectile.Fire(direction, parentDistance, parentActiveTime);
        StartCoroutine(SpawnChileProjectilet());
        StartCoroutine(PatternCooltime());
    }

    private void ActiveChildProjectile()
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
            ActiveChildProjectile();
            yield return new WaitForSeconds(parentActiveTime / childBulletCount);
        }

        for (int i = 0; i < childProjectiles.Count; i++)
        {
            childProjectiles[i].Fire(childProjectiles[i].transform.forward, childDistanece, childActiveTime);
        }
    }
    IEnumerator PatternCooltime()
    {
        readyToStart = false;
        yield return new WaitForSeconds(coolTime);
        readyToStart = true;
    }
}


