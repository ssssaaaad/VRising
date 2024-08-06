using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_MainSkillPattern2 : Pattern
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
    public float y = 10;

    private Vector3 direction;



    public float minionSpawnTime = 1;

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
        //StartCoroutine(ActiveSkill(maja.GetMinion()));
    }

    //private IEnumerator ActiveSkill(Maja_Minion minion)
    //{
       
    //}

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
