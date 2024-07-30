using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerManager PM;

    public float comboTime = 2f;    // 연속공격 유지시간
    public float attackPreDelay = 0.3f;     // 공격 전 딜레이 시간
    // 공격 이후 딜레이 시간 
    public float attack1Delay = 0.5f;
    public float attack2Delay = 0.5f;
    public float attack3Delay = 0.75f;
    public float minSpeed = 3f;     // 공격시 감소된 속도
    public float speedDuration = 0.75f;  // 속도 감소 지속시간

    public GameObject HitBox;
    public Coroutine attackCoroutain;
    public Coroutine speedCoroutain;
    public Transform attackPoint;

    private PlayerMove PlayerMove;
    private int comboCount;
    private float lastAttackTime;   // 마지막 공격시간
    private float[] comboDelay;
    private float basicSpeed;

    void Start()
    {
        Debug.Log("작동중");
        comboDelay = new float[] { attack1Delay, attack2Delay, attack3Delay };
        PlayerMove = GetComponent<PlayerMove>();
        basicSpeed = PlayerMove.playerSpeed;

        PM = GetComponent<PlayerManager>();
    }
    private bool canAttack = true;
    void Update()
    {
        lastAttackTime += Time.deltaTime;   // 마지막 공격 이후 시간 측정

        if (Input.GetButton("Fire1") && PM.CanAttack() && canAttack)     // 다른 행동을 하지 않고 클릭을 누르면
        {
            if(attackCoroutain != null)
            {
                StopCoroutine(attackCoroutain);
            }
            canAttack = false;
            PM.attacking = true;
            attackCoroutain = StartCoroutine(Attack());

            //StopCoroutine(Slowing());
        }
    }

    private IEnumerator Attack()
    {
        if (lastAttackTime > comboTime)     // 마지막 공격 이후 시간이 연속공격 유지시간 초과시 콤보 초기화
        {
            comboCount = 0;
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
            hitBox.transform.position = attackPoint.transform.position + attackPoint.transform.forward * 3;
            hitBox.transform.forward = attackPoint.transform.forward;            
            hitBox.transform.SetParent(attackPoint.transform);  // 히트박스를 Model의 자식으로 설정

            lastAttackTime = 0f;
            comboCount++;
        }
        canAttack = true;
        PM.attacking = false;
        yield return new WaitForSeconds(speedDuration - attackPreDelay);
        PlayerMove.SetSpeed(basicSpeed);
        attackCoroutain = null;
    }

    private IEnumerator Slowing()
    {
        yield return new WaitForSeconds(speedDuration);
    }

    public void CancelAttacking()
    {
        StopCoroutine(Attack());
        StopCoroutine(Slowing());
        if (PlayerMove.playerSpeed == minSpeed)
            PlayerMove.SetSpeed(basicSpeed);

        PM.attacking = false;
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

}
