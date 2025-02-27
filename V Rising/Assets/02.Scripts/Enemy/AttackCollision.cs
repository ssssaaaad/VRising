using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void CallbackEventHandler(Vector3 position);
public class AttackCollision : MonoBehaviour
{
    protected List<Transform> hitObjects = new List<Transform>();
    protected float hitCooltime = 0;
    protected float damage;
    protected bool activeAfterHit;
    bool collision = true;
    protected CallbackEventHandler callback;
    private void OnEnable()
    {
        hitObjects.Clear();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void InitAttack(float damage, bool activeAfterHit, bool collision = true, float hitCooltime = 0)
    {
        hitObjects.Clear();
        this.damage = damage;
        this.activeAfterHit = activeAfterHit;
        this.collision = collision;
        this.hitCooltime = hitCooltime;
    }

    private IEnumerator HitObjectCooltime(Transform target)
    {
        if (hitCooltime <= 0)
            yield break;
        yield return new WaitForSeconds(hitCooltime);

        if (hitObjects.Contains(target))
        {
            hitObjects.Remove(target);
        }
    }

    public void DestoryCollision(float time)
    {
        StartCoroutine(Coroutine_DestoryCollision(time));
    }

    private IEnumerator Coroutine_DestoryCollision(float time)
    {
        yield return new WaitForSeconds(0.3f);
        if( gameObject != null)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collision)
        {
            if (other.transform.CompareTag("Player"))
            {
                for (int i = 0; i < hitObjects.Count; i++)
                {
                    if (hitObjects[i] == other.transform)
                    {
                        return;
                    }
                }

                hitObjects.Add(other.transform);
                if (callback != null)
                {
                    callback(transform.position);
                }
                other.GetComponent<Playerstate>().UpdateHP(damage);

                if (hitCooltime > 0)
                {
                    StartCoroutine(HitObjectCooltime(other.transform));
                }

            }
            if (other.transform.CompareTag("Transparent"))
            {
                if (callback != null)
                {
                    callback(transform.position);
                    Destroy(gameObject);
                }
            }
        }
    }
}
