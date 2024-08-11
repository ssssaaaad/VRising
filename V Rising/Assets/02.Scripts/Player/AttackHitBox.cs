using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public float activeTime = 1f;

    private PlayerAttack PlayerAttack;
    private Playerstate PState;

    void Start()
    {
        Destroy(gameObject, activeTime);
        PlayerAttack = GetComponentInParent<PlayerAttack>();
        PState = GetComponentInParent<Playerstate>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // 만약 히트박스에 닿은 대상의 레이어가 enemy라면
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("공격!!!!!!!!");
            if (PlayerAttack.Combo() == 2)  // 만약 3타라면 hp를 damage * 1.1 만큼 감소
                PlayerAttack.Damage(other, PlayerAttack.Attack3dmg);
            else    // 3타가 아니면 상대방의 hp 를 damage 만큼 감소
                PlayerAttack.Damage(other, PlayerAttack.Attack1dmg);

            if (PlayerAttack.afterDash)
            {
                Debug.Log("강화평타");

                PlayerAttack.Damage(other, 0.25f);
                PState.UpdateHP(-PState.hp_Max * 0.1f);
                PlayerAttack.afterDash = false;
                StopCoroutine(PlayerAttack.AfterDash());
                PlayerAttack.Jansang.SetActive(false);
            }
        }
    }
}
