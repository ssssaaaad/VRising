using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerManager PM;
    private Playerstate PS;

    public float Attack1dmg = 0.35f;
    public float Attack3dmg = 0.4f;

    public float comboTime = 2f;    // 연속공격 유지시간
    public float attackPreDelay = 0.3f;     // 공격 전 딜레이 시간
    public float attack1Delay = 0.5f;       // 공격 이후 딜레이 시간 
    public float attack2Delay = 0.5f;
    public float attack3Delay = 0.75f;
    public float minSpeed = 3f;     // 공격시 감소된 속도
    public float speedDuration = 0.75f;  // 속도 감소 지속시간
    public bool afterDash = false;

    public GameObject Jansang;
    public GameObject HitBox;
    public Coroutine attackCoroutain;
    public Coroutine speedCoroutain;
    public Transform attackPoint;   // 공격판정 위치

    private PlayerMove PlayerMove;
    private int comboCount;
    private float lastAttackTime;   // 마지막 공격시간
    private float[] comboDelay;
    private float basicSpeed = 10f;
    private bool canAttack = true;

    public int cameraShakeTypeIndex = 0;

    void Start()
    {
        comboDelay = new float[] { attack1Delay, attack2Delay, attack3Delay };
        PlayerMove = GetComponent<PlayerMove>();

        PM = GetComponent<PlayerManager>();
        PS = GetComponent<Playerstate>();
    }

    
    void Update()
    {
        lastAttackTime += Time.deltaTime;   // 마지막 공격 이후 시간 측정
    }

    public void Click()
    {
        if (attackCoroutain != null)
        {
            StopCoroutine(attackCoroutain);
        }
        canAttack = false;
        PM.attacking = true;
        attackCoroutain = StartCoroutine(Attack());
    }


    private IEnumerator Attack()
    {
        if (lastAttackTime > comboTime)     // 마지막 공격 이후 시간이 연속공격 유지시간 초과시 콤보 초기화
        {
            comboCount = 0;
            PM.animator.SetBool("NormalAttackCheck", false);
        }

        Debug.Log("속도감소");
        PlayerMove.SetSpeed(minSpeed);
        speedCoroutain = StartCoroutine(Slowing());

        yield return new WaitForSeconds(attackPreDelay);    // 클릭시 attackPreDelay 만큼 기다렸다 공격

        // 첫 공격일 경우 or 마지막 공격 이후 시간이 공격 딜레이 시간 이상일 경우에 공격
        if (comboCount == 0 || lastAttackTime >= comboDelay[comboCount - 1])
        {
            if (comboCount >= 3)
                comboCount = 0;

            GameObject hitBox = Instantiate(HitBox);    // 히트박스 소환
            hitBox.transform.position = attackPoint.transform.position + attackPoint.transform.forward * 2.5f;
            hitBox.transform.forward = attackPoint.transform.forward;
            hitBox.transform.SetParent(attackPoint.transform);  // 히트박스를 Swod의 자식으로 설정

            lastAttackTime = 0f;
            comboCount++;
        }
        canAttack = true;

        if (PM != null)
        {
            PM.attacking = false;
        }
        yield return new WaitForSeconds(speedDuration - attackPreDelay);
        PlayerMove.SetSpeed(basicSpeed);
        attackCoroutain = null;
    }

    private IEnumerator Slowing()
    {
        yield return new WaitForSeconds(speedDuration);
    }

    public IEnumerator AfterDash()
    {
        afterDash = true;

        GameObject jansang = Instantiate(Jansang);
        jansang.transform.position = transform.position;
        jansang.transform.forward = attackPoint.transform.forward;
        Destroy(jansang, 3f);
        Debug.Log("평타 강화중");

        yield return new WaitForSeconds(3);

        afterDash = false;

        
        Debug.Log("평타강화 자동취소");
    }

    public void CancelAttacking()
    {
        StopCoroutine(Attack());
        StopCoroutine(Slowing());
        if (PlayerMove.playerSpeed == minSpeed)
            PlayerMove.SetSpeed(basicSpeed);

        if (PM != null)
        {
            PM.attacking = false;
        }
        Debug.Log("attack cancel");
    }

    public int Combo()
    {
        return comboCount;
    }

    public bool Canattack()
    { 
        return canAttack; 
    }


    // hit : 맞은 대상, coeff : 데미지 계수
    public void Damage(Collider hit, float coeff)
    {
        hit.GetComponentInParent<Enemy>().UpdateHP(-PS.power * coeff, PM.transform);

        CameraShakeManager.instance.ShakeSkillCall(cameraShakeTypeIndex);
    }
}
