using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_MainSkillPattern1 : Pattern
{
    public Maja maja;

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
    private Vector3 direction;

    public Projectile parentProjectile_Prefab;
    public Projectile childProjectile_Prefab;
    private Projectile childProjectile;



    private void Awake()
    {
        maja = GetComponent<Maja>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    public override void InitPattern(Maja maja)
    {
        this.maja = maja;
    }
    public override bool CooltimeCheck()
    {
        return readyToStart;
    }
    public override void ActivePattern(Vector3 direction)
    {
        if (!readyToStart)
        {
            return;
        }
        readyToStart = false;

        StartCoroutine(Coroutine_AttackDelayTime(direction));
        StartCoroutine(PatternDelayTime());
        StartCoroutine(PatternCooltime());
    }

    protected override IEnumerator Coroutine_AttackDelayTime(Vector3 direction)
    {
        yield return new WaitForSeconds(attackDelayTime);

        StartCoroutine(Coroutine_ActivePattern(direction));
    }

    private Projectile ActiveChildProjectile(Projectile parentProjectile, bool secondDirection_Right)
    {
        Projectile childProjectile = Instantiate(childProjectile_Prefab);
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
        return(childProjectile);
    }

    protected override bool GetPatternDelay()
    {
        return patterDelay;
    }

    protected override IEnumerator PatternDelayTime()
    {
        maja.PatternDelay = GetPatternDelay;
        patterDelay = true;
        yield return new WaitForSeconds(delayTime);
        patterDelay = false;

    }

    IEnumerator Coroutine_ActivePattern(Vector3 targetDirection)
    {
        
        for (int i = 0; i < 2; i++)
        {
            Projectile parentProjectile = Instantiate(parentProjectile_Prefab);
            float angle = Mathf.Atan2(targetDirection.z, targetDirection.x);
            if (i > 0)
            {
                angle += -70 * Mathf.Deg2Rad;
            }
            else
            {
                angle += 70 * Mathf.Deg2Rad;
            }

            direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            parentProjectile.transform.position = transform.position + (-direction * startPosition);

            lookAtDirection = (transform.position + direction);
            lookAtDirection.y = parentProjectile.transform.position.y;
            parentProjectile.transform.LookAt(lookAtDirection);
            parentProjectile.InitAttack(parentDamage, true, false);

            if (maja.target != null)
            {
                Vector3 enemy_Cross = Vector3.Cross(parentProjectile.transform.forward, (maja.target.position - parentProjectile.transform.position));

                if (enemy_Cross.y > 0)
                {
                    secondDirection_Right = true;
                }
                else
                {
                    secondDirection_Right = false;
                }
            }
            else
            {
                secondDirection_Right = false;
            }
            parentProjectile.Fire(direction, parentDistance, parentActiveTime);
            StartCoroutine(SpawnChileProjectilet(parentProjectile, secondDirection_Right));
            yield return new WaitForSeconds(1);
        }
        

    }

    IEnumerator SpawnChileProjectilet(Projectile parentProjectile, bool secondDirection_Right)
    {
        List<Projectile> childProjectiles = new List<Projectile>();
        for (int i = 0; i < childBulletCount; i++)
        {
            childProjectiles.Add(ActiveChildProjectile(parentProjectile, secondDirection_Right));
            yield return new WaitForSeconds(parentActiveTime / childBulletCount);
        }

        for (int i = 0; i < childProjectiles.Count; i++)
        {
            childProjectiles[i].Fire(childProjectiles[i].transform.forward, childDistanece, childActiveTime);
        }
    }

    protected override IEnumerator PatternCooltime()
    {
        readyToStart = false;
        yield return new WaitForSeconds(coolTime);
        readyToStart = true;
    }
}


