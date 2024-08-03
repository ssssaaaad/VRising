using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tskill : MonoBehaviour
{
    PlayerManager PM;
    PlayerMove PlayerMove;

    public float dashReady = 0.5f;  // 시전 준비시간
    public float ghostDashing = 1f; // 대쉬 지속시간
    public float bustTime = 1.5f;   // 폭발 지연시간
    public float cooldownTime = 20f; // 쿨타임 (초)
    public float highSpeed = 50f;   // 돌진 속도
    public float normalSpeed = 5f;  // 기본 속도

    private Vector3 head;       // 시전 방향
    private bool isCoolingDown = false;
    private float cooldownEndTime; // 쿨타임 종료 시간
    private Coroutine ghostTime;
    private bool forwardLock = false;

    List<Transform> hitObjects = new List<Transform>();

    void Start()
    {
        PM = GetComponent<PlayerManager>();
        PlayerMove = GetComponent<PlayerMove>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isCoolingDown)
        {
            // 시전
            ghostTime = StartCoroutine(Ghostdash());
        }

    }

    public IEnumerator Ghostdash()
    {
        PM.tskilling = true;
        PlayerMove.SetSpeed(0);     // 위치고정
        
        yield return new WaitForSeconds(dashReady);         // 시전시간동안 대기 

        isCoolingDown = true;

        forwardLock = true; // 방향 고정

        // 플레이어 충돌 판정 Trigger로 변경

        // 고정된 방향으로 돌진
        PlayerMove.SetSpeed(highSpeed);

        // 데미지만큼 플레이어 회복

        yield return new WaitForSeconds(ghostDashing);      // 대쉬 지속시간동안 속도 유지

        // 속도 감소
        PlayerMove.SetSpeed(normalSpeed);
        // 돌진 종료
        PM.tskilling = false;
        // 방향 고정 해제
        forwardLock = false;

        // 쿨타임 계산
        cooldownEndTime = Time.time + cooldownTime;
        while (Time.time < cooldownEndTime)
        {
            // 쿨타임 동안 대기
            yield return null;
        }

        // 쿨타임 종료
        isCoolingDown = false;
    }

    public void CancelTSkill()
    {

    }
    
    private void OnTriggerEnter(Collider other)     
    {
        if (other.CompareTag("Enemy"))      // 부딪힌 대상의 레이어 확인
        {
            StartCoroutine(hitDelayTime(other.transform));
            print("Boom");
        }
    }

    public IEnumerator hitDelayTime(Transform hitObject)
    {
        hitObjects.Add(hitObject);      // 히트대상을 리스트에 추가
        yield return new WaitForSeconds(bustTime);      // 폭발 지연시간동안 대기
        hitObjects.Remove(hitObject.transform);     // 폭발이 끝나면 리스트에서 제외
    }

    public bool ForwardLock()
    {
        return forwardLock;
    }

}
