using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_Teleport : Pattern
{
    private Maja maja;
    private Transform player;

    public float coolTime = 5;
    public bool readyToStart = true;

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
        if (maja.target == null)
            return;

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

        direction = (maja.mapOrigin.position - maja.target.position).normalized * (maja.mapRadius * 0.6f);
        transform.position = direction;
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
