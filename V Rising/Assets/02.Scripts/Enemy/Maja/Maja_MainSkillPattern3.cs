using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_MainSkillPattern3 : Pattern
{
    private Maja maja;
    public Maja_Minion minion_Prefab;
    public Transform spanwLineParticel;
    public Transform spawnParticle;
    public Transform attackPsotionCircle_Prefab;

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

        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(SpawnMinion());
            yield return new WaitForSeconds(0.5f);
        }
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

        Vector3 originDirection = (spawnPosition - maja.mapOrigin.position).normalized;
        float originDistance = Vector3.Distance(maja.mapOrigin.position, spawnPosition);
        originDistance = Mathf.Clamp(originDistance + Random.value, originDistance - 1, maja.mapRadius);
        spawnPosition = maja.mapOrigin.position + (originDirection * originDistance);
        
        direction = spawnPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;
        //}


        float distance = Vector3.Distance(startPosition, spawnPosition);
        Vector3 centerPosition = startPosition + (direction * (distance / 2));
        centerPosition.y += maja.target.position.y + y;
        Vector3 p4, p5;

        Transform line = Instantiate(spanwLineParticel, startPosition, spawnParticle.transform.rotation, null);
        Transform attackPositionCircle = Instantiate(attackPsotionCircle_Prefab, spawnPosition + Vector3.up, attackPsotionCircle_Prefab.rotation);
     
        while (time <= 1)
        {
            p4 = Vector3.Lerp(startPosition, centerPosition, time);
            p5 = Vector3.Lerp(centerPosition, spawnPosition, time);
            line.position = Vector3.Lerp(p4, p5, time);

            time += 0.01f / minionSpawnTime;

            yield return new WaitForSeconds(0.01f);
        }
        Destroy(attackPositionCircle.gameObject);

        Maja_Minion minion = Instantiate(minion_Prefab);
        minion.transform.position = spawnPosition;
        Vector3 a = (maja.mapOrigin.position - transform.position);
        a.y = 0;
        a = a.normalized;
        minion.transform.forward = a;

        minion.InitEnemy(maja);
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
