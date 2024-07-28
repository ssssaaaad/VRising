using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maja_Teleport : MonoBehaviour, Pattern
{
    private Maja maja;
    private Transform player;

    public float coolTime = 5;
    public bool readyToStart = true;
    public float startDistance = 2;
    public float width = 5;

    public Projectile projectile_Prefab;

    public bool start = false;
    public void InitPattern(Maja maja)
    {
        this.maja = maja;
    }

    public void ActivePattern(Vector3 direction)
    {
        maja.StopMoveTarget();
        if (maja.target == null)
            return;
        direction = (maja.mapOriginPosition.position - maja.target.position).normalized * (maja.mapRadius * 0.6f);
        transform.position = direction;

        StartCoroutine(PatternCooltime());
    }
    IEnumerator PatternCooltime()
    {
        readyToStart = false;
        yield return new WaitForSeconds(coolTime);
        readyToStart = true;
    }
}
