using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private Tskill Tskill;

    void Start()
    {
        Tskill = GetComponentInParent<Tskill>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))      // 부딪힌 대상의 레이어 확인
        {
            Tskill.StartCoroutine(Tskill.hitDelayTime(other.transform));
        }
    }
}
