using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Maja_NormalSkillPattern1 : Pattern
{
    private Maja maja;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float startDistance = 2;
    public float width = 5;

    public float attackDistance = 10;
    public float attackActiveTime = 3;

    public int bulletCount = 4;

    public Ease ease = Ease.Linear;

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
    public override void SetDamage(float dmg)
    {
        if (dmg > damage_Max)
        {
            dmg = damage_Max;
        }
        else if (dmg < damage_Min)
        {
            dmg = damage_Min;
        }
        damage = dmg;
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

        if (maja.talkSound == null)
        {
            maja.talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Talk_Skill, transform, Vector3.zero);
        }

        maja.animator.SetTrigger("NormalSkillPattern");
        StartCoroutine(Coroutine_AttackPattern(direction));
        StartCoroutine(PatternDelayTime());
        StartCoroutine(PatternCooltime());
        for (int i = 0; i < vfxList.Length; i++)
        {
            StartCoroutine(VFXAcitve(vfxList[i]));
        }
    }
    protected override bool GetPatternDelay()
    {
        return patterDelay;
    }

    protected override IEnumerator Coroutine_AttackPattern(Vector3 direction)
    {
        yield return new WaitForSeconds(attackDelayTime);
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_NormalSkill1, transform, Vector3.zero);
        Vector3 right = Vector3.Cross(direction, Vector3.up);
        for (int i = 0; i < bulletCount; i++)
        {
            projectile = Instantiate(projectile_Prefab);
            spawnPosition = transform.position + (((right * width) / (bulletCount - 1)) * i);
            spawnPosition += -right * width / 2;
            projectile.transform.position = spawnPosition + direction * startDistance;
            projectile.transform.LookAt(projectile.transform.position + direction);
            projectile.InitAttack(damage, true);
            projectile.Fire(direction, attackDistance, attackActiveTime, ease);
        }
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
    protected override IEnumerator VFXAcitve(VFX vfx)
    {
        yield return new WaitForSeconds(vfx.startTime);
        vfx.vfxObject.SetActive(true);
        yield return new WaitForSeconds(vfx.operatingTime);
        vfx.vfxObject.SetActive(false);
    }
}
