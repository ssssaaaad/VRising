using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Maja_NormalSkillPattern2 : Pattern
{
    private Maja maja;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float radius = 5;
    public float damage = 10;

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
        StartCoroutine(Coroutine_AttackPattern(direction));
        StartCoroutine(PatternDelayTime());
        StartCoroutine(PatternCooltime());
    }
    protected override bool GetPatternDelay()
    {
        return patterDelay;
    }

    protected override IEnumerator Coroutine_AttackPattern(Vector3 direction)
    {
        yield return new WaitForSeconds(attackDelayTime);
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, radius, LayerMask.NameToLayer("Player"));
        for (int i = 0; i < hitTargets.Length; i++)
        {
            //hitTargets[i].GetComponent<플레이어 hp > ().업데이트 hp(damage);
        }
        Vector2 randomPosition = Random.insideUnitCircle * Random.Range(5,10);
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += randomPosition.x;
        spawnPosition.z += randomPosition.y;
        float distance = Vector3.Distance(maja.mapOrigin.position, spawnPosition);
        if (distance > maja.mapRadius)
        {
            spawnPosition += (maja.mapOrigin.position - spawnPosition).normalized * (distance + Random.Range(1,3) - radius);
        }

        maja.SpawnMinion(spawnPosition);

        yield return new WaitForSeconds(0.3f);
    }

    protected override IEnumerator PatternDelayTime()
    {
        maja.PatternDelay = GetPatternDelay;
        patterDelay = true;
        yield return new WaitForSeconds(delayTime);
        patterDelay = false;
    }

    protected override IEnumerator PatternCooltime()
    {
        readyToStart = false;
        yield return new WaitForSeconds(coolTime);
        readyToStart = true;
    }

}
