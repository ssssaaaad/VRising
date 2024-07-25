using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    protected List<Transform> hitObjects = new List<Transform>();
    protected float hitCooltime = 0;
    protected float damage;
    protected bool activeAfterHit;
    bool collision = true;

    private void OnEnable()
    {
        hitObjects.Clear();
    }

    protected void InitAttack(float damage, bool activeAfterHit, bool collision = true, float hitCooltime = 0)
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

                // 플레이어 데미지 추가

                if (hitCooltime > 0)
                {
                    StartCoroutine(HitObjectCooltime(other.transform));
                }

            }
        }
    }
}
