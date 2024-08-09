using NHance.Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class Projectile : AttackCollision
{
    private Vector3 direction;
    private float distance;
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
            transform.position = Vector3.Lerp(startPosition, startPosition + direction * distance, currentTime);
            if(currentTime == 1)
            {
                gameObject.SetActive(false);
                // 제거 효과
                Destroy(gameObject,2);
            }
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }


    public new void InitAttack(float damage, bool activeAfterHit, bool collision = true ,float hitCooltime = 0)
    {
        base.InitAttack(damage, activeAfterHit, collision, hitCooltime);
        fire = false;
        currentTime = 0;
    }

    public void Fire(Vector3 direction, float distance, float activeTime, Ease ease, CallbackEventHandler callbackEvent = null, float delayTime = 0)
    {
        if (fire)
        {
            return;
        }

        this.direction = direction;
        this.distance = distance;
        this.activeTime = activeTime;
        this.callback = callbackEvent;

        startPosition = transform.position;
        gameObject.SetActive(true);

        if(delayTime > 0)
        {
            StartCoroutine(FireDelayTime(delayTime, ease));
        }
        else
        {
            ActiveCurrentTIme(ease);
            fire = true;
        }
    }


    IEnumerator FireDelayTime(float delayTime, Ease ease)
    {
        yield return new WaitForSeconds(delayTime);
        ActiveCurrentTIme(ease);
        fire = true;
    }

    private void ActiveCurrentTIme(Ease ease)
    {
        currentTime = 0;
        DOTween.To(() => currentTime, x => currentTime = x, 1, activeTime).SetEase(ease);
    }
}
