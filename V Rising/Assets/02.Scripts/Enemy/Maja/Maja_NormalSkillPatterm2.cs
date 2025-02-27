using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Maja_NormalSkillPattern2 : Pattern
{
    private Maja maja;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float radius = 5;
    public LayerMask layerMask;

    public Projectile projectile_Prefab;
    private Projectile projectile;

    private void OnDestroy()
    {
        StopAllCoroutines();
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

        if (maja.talkSound == null)
        {
            maja.talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Talk_Skill, transform, Vector3.zero);
        }
        maja.animator.SetTrigger("NormalSkillPattern2");
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
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_NormalSkill2_Start, transform, Vector3.zero);
        yield return new WaitForSeconds(attackDelayTime);
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Explosion, transform, Vector3.zero);
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, radius, layerMask);
        for (int i = 0; i < hitTargets.Length; i++)
        {
            hitTargets[i].GetComponent<Playerstate>().UpdateHP(damage);
        }
        Vector2 randomPosition = Random.insideUnitCircle * Random.Range(5,10);
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += randomPosition.x;
        spawnPosition.z += randomPosition.y;
        float distance = Vector3.Distance(maja.origin.position, spawnPosition);
        if (distance > maja.mapRadius)
        {
            spawnPosition += (maja.origin.position - spawnPosition).normalized * (distance + Random.Range(1,3) - radius);
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
    protected override IEnumerator VFXAcitve(VFX vfx)
    {
        yield return new WaitForSeconds(vfx.startTime);
        if (!vfx.localPosition)
        {
            vfx.vfxObject.transform.SetParent(maja.effectPosition);
            vfx.vfxObject.transform.localPosition = Vector3.zero;
            vfx.vfxObject.transform.SetParent(null);
        }
        vfx.vfxObject.SetActive(true);
        yield return new WaitForSeconds(vfx.operatingTime);
        vfx.vfxObject.SetActive(false);
    }
}
