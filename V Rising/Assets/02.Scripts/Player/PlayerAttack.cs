using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    int comboCount;
    // 마지막 공격시간
    float lastAttackTime;

    // 연속공격 유지시간
    public float comboTime = 2f;

    public AttackHitBox attackHitBox;
    // 공격 이후 딜레이 시간 
    public float attack1Delay = 0.5f;
    public float attack2Delay = 0.5f;
    public float attack3Delay = 0.75f;

    float[] comboDelay;

    public float attackDamage = 10f;

    public GameObject HitBox;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("작동중");
        comboDelay = new float[] { attack1Delay, attack2Delay, attack3Delay };
    }

    // Update is called once per frame
    void Update()
    {
        // 마지막 공격 이후 시간 측정
        lastAttackTime += Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            // 마지막 공격 이후 시간이 연속공격 유지시간 초과시 콤보 초기화
            if (lastAttackTime > comboTime)
            {
                Debug.Log("콤보 초기화");
                lastAttackTime = 0f;
                comboCount = 0;
            }

            // 첫 공격일 경우 or 마지막 공격 이후 시간이 공격 딜레이 시간 이상일 경우에 공격
            if (comboCount == 0 || lastAttackTime >= comboDelay[comboCount - 1])
            {
                if (comboCount >= 3)
                    comboCount = 0;

                switch (comboCount)
                {
                    case 0:
                        Debug.Log("1타");
                        lastAttackTime = 0f;
                        comboCount++;
                        break;
                    case 1:
                        Debug.Log("2타");
                        lastAttackTime = 0f;
                        comboCount++;
                        break;
                    case 2:
                        Debug.Log("3타");
                        lastAttackTime = 0f;
                        comboCount++;
                        break;
                }
            }
        }
    }
}
