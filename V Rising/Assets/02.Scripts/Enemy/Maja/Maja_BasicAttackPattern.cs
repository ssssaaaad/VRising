using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Maja_BasicAttackPattern : Pattern
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
    public Ease ease = Ease.Linear;

    private Vector3 spawnPosition;

    public Projectile projectile_Prefab;
    private Projectile projectile;

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
            maja.talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Talk_Default, transform, Vector3.zero);
        }
        maja.animator.SetTrigger("BasicAttackPattern");

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
        Vector3 right = Vector3.Cross(direction, Vector3.up);
        projectile = Instantiate(projectile_Prefab);
        projectile.transform.position = transform.position + direction * startDistance;
        projectile.transform.LookAt(projectile.transform.position + direction);
        projectile.InitAttack(damage, false);
        projectile.Fire(direction, attackDistance, attackActiveTime, ease, new CallbackEventHandler(maja.SpawnMinion));

        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_BasicAttack, projectile.transform, Vector3.zero);
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
