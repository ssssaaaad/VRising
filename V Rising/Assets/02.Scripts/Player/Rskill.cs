using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rskill : MonoBehaviour
{
    private PlayerManager PM;

    public GameObject skillPrefab; // 발사할 스킬(발사체) 프리팹
    public Transform firePoint; // 스킬이 발사될 위치
    public float skillSpeed = 20f; // 발사체의 속도
    public float castTime = 1f; // 시전 시간 (초)
    public float cooldownTime = 8f; // 쿨타임 (초)
    public float slowSpeed = 0.5f; // 시전 시 캐릭터 속도 감소
    public float DestroyBullet = 1f; // 불렛이 부셔지는 시간
    public SkillUI skillUI;
    private float playerSpeed; // 정상 캐릭터 속도
    private PlayerMove playerMove; // PlayerMove 스크립트 참조
    private bool isCoolingDown = false; // 쿨타임 여부 확인
    private float cooldownEndTime; // 쿨타임 종료 시간
    private Coroutine castingCoroutine;
    void Start()
    {
        // PlayerMove 컴포넌트 가져오기
        playerMove = GetComponent<PlayerMove>();
        PM = GetComponent<PlayerManager>();
        playerSpeed = playerMove.playerSpeed;
    }

    void Update()
    {
        if (PM != null)
        {
            if (Input.GetKeyDown(KeyCode.R) && !isCoolingDown && PM.CanRskill())
            {

                Debug.Log("Starting skill casting.");
                castingCoroutine = StartCoroutine(CastSkill());
            }
        }
    }

    public IEnumerator CastSkill()
    {
        PM.rskilling = true; //시전 상태로 설정
        isCoolingDown = true;

        // 시전 시간 동안 캐릭터 속도 감소
        if (playerMove != null)
        {
            Debug.Log("Applying slowed speed.");
            playerMove.SetSpeed(slowSpeed);
        }

        // 시전 시간 동안 기다리기
        yield return new WaitForSeconds(castTime);

        // 발사체 발사

        ActivateSkill();

        // 시전 시간이 끝난 후 캐릭터 속도 원래대로 복원
        
        playerMove.SetSpeed(playerSpeed);
        skillUI.coolTimeImage();
        PM.rskilling = false;

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

    public void CancelRCasting()
    {
        if (PM.rskilling)
        {
            Debug.Log("Cancelling skill casting.");
            if (castingCoroutine != null)
            {
                StopCoroutine(castingCoroutine);
                castingCoroutine = null;
            }
            PM.rskilling = false;

            // 시전 중 속도 복원
            if (playerMove != null)
            {
                Debug.Log("Restoring normal speed after cancel."); // 속도 복원 로그
                playerMove.SetSpeed(playerSpeed); // 속도 복원

                // 쿨타임 리셋: 취소 시 쿨타임을 0으로 설정하여 즉시 사용 가능하게 함
                cooldownEndTime = Time.time; // 즉시 사용 가능하게 설정
                isCoolingDown = false;
            }
            
        }
    }
        void ActivateSkill()
    {
        if (skillPrefab != null && firePoint != null)
        {
            // 스킬(발사체) 생성
            GameObject skill = Instantiate(skillPrefab, firePoint.position, firePoint.rotation);

            // 발사체의 방향을 발사 방향으로 설정
            Vector3 fireDirection = firePoint.forward;
            skill.transform.forward = fireDirection;

            // 발사체에 속도를 설정
            Rskillbullet projectile = skill.GetComponent<Rskillbullet>();
            if (projectile != null)
            {
                projectile.speed = skillSpeed; // 발사체의 속도 조정
            }

            // 발사 후 스킬을 자동으로 비활성화
            Destroy(skill, DestroyBullet); // 발사체를 5초 후에 파괴 (필요에 따라 조정)
        }
    }
    public bool IsCasting()
    {
        return PM.rskilling; // 현재 스킬이 시전 중인지 여부 반환
    }
}

