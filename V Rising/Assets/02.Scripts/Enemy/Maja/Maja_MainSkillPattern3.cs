using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Maja_MainSkillPattern3 : Pattern
{
    private Maja maja;
    public Maja_Minion minion_Prefab;
    public Transform spanwLineParticel;
    public Transform spawnParticle;
    public Transform attackPsotionCircle_Prefab;
    public LayerMask layerMask;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float startDistance = 2;
    public float width = 5;

    public float attackDistance = 10;
    public float attackActiveTime = 3;

    public int bulletCount = 4;

    public float y = 20;

    private Vector3 direction;

    public Ease ease;

    public float minionSpawnTime = 1;

    public bool start = false;
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
        maja.animator.SetTrigger("MainSkillPattern3");
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
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_MainSkill3_Start, transform, Vector3.zero);
        yield return new WaitForSeconds(attackDelayTime);

        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_Explosion, transform, Vector3.zero);
        yield return new WaitForSeconds(2f);
        if (maja.hp_Current == 0)
        {
            yield break;
        }
        maja.model.gameObject.SetActive(false);

        for (int i = 0; i < bulletCount; i++)
        {
            if (maja.hp_Current == 0)
            {
                maja.model.gameObject.SetActive(true);
                maja.animator.SetTrigger("Groggy");
                yield break;
            }

            StartCoroutine(SpawnMinion());
            yield return new WaitForSeconds(1f);
        }

        maja.model.gameObject.SetActive(true);
        maja.animator.SetTrigger("Spawn");
    }

    private IEnumerator SpawnMinion()
    {
        float time = 0f;
        float angle = 0;
        Vector3 startPosition = transform.position;
        //direction = maja.target.position - transform.position;
        //direction.y = 0;
        //direction = direction.normalized;
        Vector3 spawnPosition = maja.target.position;

        //if(Random.value < 0.5)
        //{

        Vector2 random = Random.insideUnitCircle * Random.Range(1,6);
        spawnPosition += new Vector3(random.x, 0, random.y);

        Vector3 originDirection = (spawnPosition - maja.origin.position).normalized;
        float originDistance = Vector3.Distance(maja.origin.position, spawnPosition);
        originDistance = Mathf.Clamp(originDistance + Random.value, originDistance - 1, maja.mapRadius);
        spawnPosition = maja.origin.position + (originDirection * originDistance);
        
        direction = spawnPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;
        //}


        float distance = Vector3.Distance(startPosition, spawnPosition);
        Vector3 centerPosition = startPosition + (direction * (distance / 2));
        centerPosition.y += maja.target.position.y + y;
        Vector3 p4, p5;
        Sequence sequence = DOTween.Sequence().Append(DOTween.To(() => time, x => time = x, 0.5f, 0.5f).SetEase(Ease.OutQuad)).
            Append(DOTween.To(() => time, x => time = x, 1f, 0.5f).SetEase(Ease.OutQuad));
        Transform line = Instantiate(spanwLineParticel, startPosition, spawnParticle.transform.rotation, null);
        Transform attackPositionCircle = Instantiate(attackPsotionCircle_Prefab, spawnPosition + Vector3.up, attackPsotionCircle_Prefab.rotation);

        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.Boss_MainSkill3_Throw, transform, Vector3.zero);
        while (time < 1)
        {
            p4 = Vector3.Lerp(startPosition, centerPosition, time);
            p5 = Vector3.Lerp(centerPosition, spawnPosition, time);
            line.position = Vector3.Lerp(p4, p5, time);
            yield return new WaitForSeconds(0.01f);
        }
        p4 = Vector3.Lerp(startPosition, centerPosition, time);
        p5 = Vector3.Lerp(centerPosition, spawnPosition, time);
        line.position = Vector3.Lerp(p4, p5, time);
        Destroy(attackPositionCircle.gameObject);
        Destroy(line.gameObject);

        Collider[] hitTargets = Physics.OverlapSphere(spawnPosition, width, layerMask);
        for (int i = 0; i < hitTargets.Length; i++)
        {
            hitTargets[i].GetComponent<Playerstate>().UpdateHP(damage);
        }

        Maja_Minion minion = Instantiate(minion_Prefab);
        minion.transform.position = spawnPosition;
        Vector3 a = (maja.origin.position - transform.position);
        a.y = 0;
        a = a.normalized;
        minion.transform.forward = a;

        minion.InitEnemy(maja, minion.damage_Min);
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
