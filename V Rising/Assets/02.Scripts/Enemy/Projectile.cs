using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackCollision
{
    private Vector3 direction;
    private float distance;
    private bool activeAfterHit;
    private bool fire = false;

    private float currentTime = 0;
    private float activeTime;
    private Vector3 startPosition;

    private void OnEnable()
    {
        fire = false;
    }

    private void Update()
    {
        if (fire)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, startPosition + direction * distance, currentTime / activeTime);
        }
    }

    public new void InitAttack(float damage, bool activeAfterHit, float hitCooltime = 0)
    {
        base.InitAttack(damage, activeAfterHit, hitCooltime);
        fire = false;
        currentTime = 0;
    }

    public void Fire(Vector3 direction, float distance, float activeTime, float delayTime = 0)
    {
        if (fire)
        {
            return;
        }

        this.direction = direction;
        this.distance = distance;
        this.activeTime = activeTime;

        startPosition = transform.position;
        gameObject.SetActive(true);

        if(delayTime > 0)
        {
            StartCoroutine(FireDelayTime(delayTime));
        }
        else
        {
            fire = true;
        }
    }


    IEnumerator FireDelayTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        fire = true;
    }
}
