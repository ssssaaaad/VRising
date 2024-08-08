using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Qskill : MonoBehaviour
{
    private PlayerManager PM;
    private Playerstate PS;
    private Coroutine spinTime;

    public GameObject Qspin;        // q스킬 히트박스

    public float Qdmg = 0.35f;

    public float spinDuration = 2f; // 회전 지속시간
    public float cooldownTime = 8f; // 쿨타임 (초)
    public float hitFrequency = 0.3f;   // 다단히트 적용시간

    private bool isCoolingDown = false;
    private float cooldownEndTime; // 쿨타임 종료 시간


    void Start()
    {
        PM = GetComponent<PlayerManager>();
        PS = GetComponent<Playerstate>();
    }


    public void Q()
    {
        if(!isCoolingDown)
            spinTime = StartCoroutine(Spining());
    }

    public void ActiveSkill()
    {
        StartCoroutine(Spining());
    }

    public IEnumerator Spining()
    {
        Debug.Log("Starting Q skill casting");

        PM.qskilling = true;
        isCoolingDown = true;

        GameObject qSpin = Instantiate(Qspin);    // 히트박스 소환
        qSpin.transform.position = transform.position;
        qSpin.transform.forward = transform.forward;
        qSpin.transform.SetParent(transform);  // 히트박스를 Model의 자식으로 설정

        yield return new WaitForSeconds(spinDuration);      // q스킬 지속시간

        Destroy(qSpin);         // 히트박스 비활성화

        Debug.Log("Finishing Q skill casting.");

        PM.qskilling = false;

        // 쿨타임 설정
        cooldownEndTime = Time.time + cooldownTime;
        while (Time.time < cooldownEndTime)
        {
            // 쿨타임 동안 대기
            yield return null;
        }

        // 쿨타임 종료
        isCoolingDown = false;

    }

    public bool IsQCoolTime()
    {
        return isCoolingDown;
    }

    public void CancelQSkill()
    {
        StopCoroutine(Spining());

        Destroy(Qspin);     // 히트박스 비활성화

        PM.qskilling = false;
    }

    // hit : 맞은 대상, coeff : 데미지 계수
    public void Damage(Collider hit, float coeff)
    {
        hit.GetComponentInParent<Enemy>().UpdateHP(-PS.power * coeff);
    }

}
