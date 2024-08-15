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
    public override void SetDamage(float dmg)
    {
        return;
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
        if (maja.maja_Minions.Count == 0)
            return;
        readyToStart = false;
        if (maja.talkSound == null)
        {
            maja.talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Talk_Skill, transform, Vector3.zero);
        }
        maja.PatternDelay = GetPatternDelay;
        patterDelay = true;

        maja.animator.SetTrigger("MainSkillPattern2");
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
        maja.forward = minionPosition;
        minionPosition *= startDistance;
        minionPosition += maja.origin.position;
        minion.SetPosition_MajaMainSkill2(minionPosition);
        for (int i = 0; i < vfxList.Length; i++)
        {
            vfxList[i].vfxObject.SetActive(true);
        }
        print(minion.gameObject.name);
        while (true)
        {
            if (minion == null || maja.maja_Minions.Count == 0)
            {
                patterDelay = false;
                maja.animator.SetTrigger("Cancle");
                StartCoroutine(InactiveAllVFX(0.5f));
                yield break;
            }
            if (minion.state != Maja_Minion.State.MajaMainSkill_2)
            {
                patterDelay = false;
                maja.animator.SetTrigger("Cancle");
                StartCoroutine(InactiveAllVFX(0.5f));
                yield break;
            }
            if (Vector3.Distance(new Vector3(minion.transform.position.x, 0, minion.transform.position.z), new Vector3(minionPosition.x, 0, minionPosition.z)) < 0.5f)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        minion.ActiveMajaMainSkill2(minionSpeed);
        SFXAudioSource temp = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_MainSkill2, transform, Vector3.zero);
        while (true)
        {
            if (!temp.audioSource.isPlaying)
            {
                temp = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_MainSkill2, transform, Vector3.zero);
            }
            if (minion == null || maja.maja_Minions.Count == 0)
            {
                patterDelay = false;
                maja.animator.SetTrigger("Cancle");
                StartCoroutine(InactiveAllVFX(0.5f));
                temp.StopSound_FadeOut();
                temp = null;
                yield break;
            }
            if (minion.state == Maja_Minion.State.Death)
            {
                patterDelay = false;
                maja.animator.SetTrigger("Cancle");
                StartCoroutine(InactiveAllVFX(0.5f));
                temp.StopSound_FadeOut();
                temp = null;
                yield break;
            }
            if (Vector3.Distance(new Vector3(minion.transform.position.x, 0, minion.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) < 2f)
            {
                temp.StopSound_FadeOut();
                temp = null;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (temp != null)
        {
            temp.StopSound_FadeOut();
        }
        minion.UpdateHP(-1000, null, false);
        maja.UpdateHP(maja.hp_Max * 0.1f, null);
        yield return new WaitForSeconds(0.5f);
        patterDelay = false;
        maja.animator.SetTrigger("Cancle");

        StartCoroutine(InactiveAllVFX(0.5f));

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
    protected override IEnumerator VFXAcitve(VFX vfx)
    {
        yield return new WaitForSeconds(vfx.startTime);
        vfx.vfxObject.SetActive(true);
        yield return new WaitForSeconds(vfx.operatingTime);
        vfx.vfxObject.SetActive(false);
    }

    protected IEnumerator InactiveAllVFX(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < vfxList.Length; i++)
        {
            vfxList[i].vfxObject.SetActive(false);
        }
    }
}
