using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_AttackPattern1 : Pattern
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

    public bool start = false;
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
    protected override bool GetPatternDelay()
    {
        return patterDelay;
    }
    protected override IEnumerator Coroutine_AttackDelayTime(Vector3 direction)
    {
        yield return new WaitForSeconds(attackDelayTime);
        Vector3 right = Vector3.Cross(direction, Vector3.up);
        projectile = Instantiate(projectile_Prefab);
        projectile.transform.position = transform.position + direction * startDistance;
        projectile.transform.LookAt(projectile.transform.position + direction);
        projectile.InitAttack(damage, false);
        projectile.Fire(direction, attackDistance, attackActiveTime);
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
