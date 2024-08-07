using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Maja_MainSkillPattern2 : Pattern
{
    private Maja maja;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float startDistance = 10;
    public float minionSpeed = 2.5f;



    public float minionSpawnTime = 1;

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
        maja.PatternDelay = GetPatternDelay;
        patterDelay = true;
        StartCoroutine(Coroutine_AttackPattern(direction));
        StartCoroutine(PatternCooltime());
    }
    protected override bool GetPatternDelay()
    {
        return patterDelay;
    }
    protected override IEnumerator Coroutine_AttackPattern(Vector3 direction)
    {
        Maja_Minion minion = maja.GetCloseMinion();
        if (minion == null)
            yield break;
        Vector3 minionPosition = (minion.transform.position - transform.position).normalized;
        minionPosition *= startDistance;
        minionPosition += maja.mapOrigin.position;
        minion.SetPosition_MajaMainSkill2(minionPosition);

        print(minion.gameObject.name);
        while (true)
        {
            if (minion == null)
            {
                patterDelay = false;
                yield break;
            }
            if (minion.state == Maja_Minion.State.Death)
            {
                patterDelay = false;
                yield break;
            }
            if(Vector3.Distance(new Vector3(minion.transform.position.x , 0 , minion.transform.position.z), new Vector3(minionPosition.x, 0, minionPosition.z)) < 0.5f)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        minion.ActiveMajaMainSkill2(minionSpeed);
        while (true)
        {
            if(minion == null)
            {
                patterDelay = false;
                yield break;
            }
            if (minion.state == Maja_Minion.State.Death)
            {
                patterDelay = false;
                yield break;
            }
            if (Vector3.Distance(new Vector3(minion.transform.position.x, 0, minion.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) < 2f)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        minion.UpdateHP(-1000, false);
        maja.UpdateHP(maja.hp_Max * 0.1f);
        yield return new WaitForSeconds(0.5f);
        patterDelay = false;
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
