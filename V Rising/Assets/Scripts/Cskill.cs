using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cskill : MonoBehaviour
{
    private PlayerManager PM;

    public float slowDuration = 1.5f; // C스킬의 속도 감소 지속 시간
    public float slowSpeed = 1f; // C스킬 시속도 감소 값
    public float cooldownTime = 10f; // 쿨타임 (초)
    public float pushBackForce = 5f; // 적 오브젝트를 밀어내는 힘
    public float playerSpeed = 5f;

    // 입력이 오면 Manager 에게 가능 여부를 물어봄
    
    private bool isCastingRSkill = false; // R 스킬 시전 중 여부
    private Rskill Rskill;
    private bool isCasting = false;
    private bool isCoolingDown = false;
    private float cooldownEndTime;
    private PlayerMove playerMove;
    private Coroutine castingCoroutine;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        Rskill = GetComponent<Rskill>();
        PM = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isCoolingDown && PM.CanCskill())
        {
            Debug.Log("Starting C skill casting.");
            StartCoroutine(CastSkill());
        }
    }

    public IEnumerator CastSkill()
    {
        PM.cskilling = true;
        isCoolingDown = true;

        if (Rskill != null && Rskill.IsCasting())
        {
            Rskill.CancelRCasting();
        }
        // 시전 시간 동안 캐릭터 속도 감소
        if (playerMove != null)
        {
            Debug.Log("Applying slowed speed.");
            playerMove.SetSpeed(slowSpeed);
        }

        // 시전 시간 동안 기다리기
        yield return new WaitForSeconds(slowDuration);

        // 시전 시간이 끝난 후 캐릭터 속도 원래대로 복원
        if (playerMove != null)
        {
            playerMove.SetSpeed(playerSpeed); // 원래 속도로 복원
        }

        // 쿨타임 설정
        cooldownEndTime = Time.time + cooldownTime;
        while (Time.time < cooldownEndTime)
        {
            // 쿨타임 동안 대기
            yield return null;
        }

        // 쿨타임 종료
        isCoolingDown = false;
        PM.cskilling = false;

        
    }

    public void CancelCasting()
    {
        if (PM.cskilling)
        {
            Debug.Log("Cancelling skill casting.");
            if (castingCoroutine != null)
            {
                StopCoroutine(castingCoroutine);
                castingCoroutine = null;
            }
            PM.cskilling = false;

            // 시전 중 속도 복원
            if (playerMove != null)
            {
                Debug.Log("Restoring normal speed after cancel."); // 속도 복원 로그
                playerMove.SetSpeed(playerSpeed); // 속도 복원

                
            }

        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (PM.cskilling && other.CompareTag("Enemy"))
        {
            // 적에게 피해를 주고 밀어내는 로직
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.y = 0; // 수직 방향 무시
                enemyRb.AddForce(pushDirection.normalized * pushBackForce, ForceMode.Impulse);
            }

            // 적의 공격 무시
            // 이 부분은 적의 공격 구현에 따라 다를 수 있음
        }
    }
    */

    public bool IsCasting()
    {
        return isCasting; // 현재 스킬이 시전 중인지 여부 반환
    }
    public bool IsCastingRSkill()
    {
        return isCastingRSkill;
    }
    public void SetCastingRSkill(bool value)
    {
        isCastingRSkill = value;
    }
}
