using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_AttackPattern3 : Pattern
{
    private Maja maja;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float startDistance = 2;
    public float width = 5;

    public float attackDistance = 10;
    public float attackActiveTime = 3;

    public int bulletCount = 4;

    public float damage = 10;

    private Vector3 spawnPosition;

    public Projectile projectile_Prefab;
    private Projectile projectile;

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
        Vector3 right = Vector3.Cross(direction, Vector3.up);
        for (int i = 0; i < bulletCount; i++)
        {
            projectile = Instantiate(projectile_Prefab);
            spawnPosition = transform.position + (((right * width) / (bulletCount-1)) * i);
            spawnPosition += -right * width / 2;
            projectile.transform.position = spawnPosition + direction * startDistance;
            projectile.transform.LookAt(projectile.transform.position + direction);
            projectile.InitAttack(damage, true);
            projectile.Fire(direction, attackDistance, attackActiveTime);
        }
        StartCoroutine(PatternDelayTime());
        StartCoroutine(PatternCooltime());
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

    IEnumerator PatternCooltime()
    {
        readyToStart = false;
        yield return new WaitForSeconds(coolTime);
        readyToStart = true;
    }

}
