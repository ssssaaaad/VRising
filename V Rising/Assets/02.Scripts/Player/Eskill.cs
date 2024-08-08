using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Eskill : MonoBehaviour
{
    private PlayerManager PM;
    private Playerstate PS;
    private Coroutine castingCoroutine;
    private PlayerMove playerMove;
    private Collider target;
    private CharacterController cc;

    public GameObject E_Skill_Particle1;
    public GameObject E_Skill_Particle2;
    public GameObject skillPrefab;  // 발사할 스킬(발사체) 프리팹
    public Transform firePoint;     // 스킬이 발사될 위치
    public SkillUI skillUI;

    public float Edmg = 0.7f;           // 검기 데미지
    public float EComdodmg = 0.25f;     // 추가타 데미지

    public float skillSpeed = 30f;  // 발사체의 속도
    public float castTime = 1f;     // 시전 시간 (초)
    public float cooldownTime = 8f; // 쿨타임 (초)
    public float slowSpeed = 0.5f;  // 시전 시 캐릭터 속도 감소
    public float DestroyBullet = 1f;    // 검기 부셔지는 시간
    public float DmgDelay = 0.5f;   // 재시전 데미지 딜레이
    public float comEspeed = 30f;   // 재시전 e 돌진속도
    public string noHitPlayer = "NoHitPlayer";      // 피격판정이 없는 레이어

    private float playerSpeed;  // 정상 캐릭터 속도
    private float cooldownEndTime;  // 쿨타임 종료 시간
    private float activeTime = 1.5f;        // comboEskill 활성화 시간
    private bool isCoolingDown = false;     // 쿨타임 여부 확인
    private bool comEActive = false;    // ComEskill 활성화 여부
    private int originalLayer;      // 기존 레이어

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        PM = GetComponent<PlayerManager>();
        PS = GetComponent<Playerstate>();
        playerSpeed = playerMove.playerSpeed;
        cc = GetComponent<CharacterController>();
        originalLayer = gameObject.layer;
    }


    public void E()
    {
        Debug.Log("Eskill 시전");
        if (E_Skill_Particle1 != null)
        {
            E_Skill_Particle1.SetActive(true);
        }
        castingCoroutine = StartCoroutine(CastSkill());
    }

    public IEnumerator CastSkill()
    {
        PM.eskilling = true;
        isCoolingDown = true;

        playerMove.SetSpeed(slowSpeed);     // 이동속도 감소
        
        yield return new WaitForSeconds(castTime);  // 캐스팅 시간동안 대기

        ActivateSkill();     // 투사체 발사

        playerMove.SetSpeed(playerSpeed);   // 속도 정상화
        if(E_Skill_Particle1 != null)
            E_Skill_Particle1.SetActive(false);
        PM.eskilling = false;

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

    public void ComboEAct(Collider target)
    {
        Debug.Log("콤보 e 활성화");
        comEActive = true;
        this.target = target;
        StartCoroutine(UnActive());
    }

    public IEnumerator UnActive()
    {
        yield return new WaitForSeconds(activeTime);
        comEActive = false;
        Debug.Log("콤보 e 비활성화");
    }

    public void ComboE()
    {
        comEActive = false;
        StopCoroutine(UnActive());
        // 재시전 E 
        if (target == null)
            return;
        if (!target.GetComponentInParent<Enemy>().alive)
            return;


        PM.comeskilling = true;
        if (E_Skill_Particle2 != null)
        {
            E_Skill_Particle2.SetActive(true);
        }
        StartCoroutine(ComboECoroutine());

        Debug.Log("ComdoE 시전");
        PM.comeskilling = false;
    }

    public IEnumerator ComboECoroutine()
    {
        // 충돌 비활성화
        gameObject.layer = LayerMask.NameToLayer(noHitPlayer);

        Vector3 startPosition = transform.position;
        float check = 0;
        while (check < 1)
        {
            transform.position = Vector3.Lerp(startPosition, target.transform.position, check);
            check += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        check = 0;
        float attackTime = 0.3f;
        float attackCheck = 0;
        while (check < 1)
        {
            transform.position = target.transform.position;
            check += 0.01f;
            attackCheck += 0.01f;
            if(attackCheck > attackTime)
            {
                Damage(target, EComdodmg);
                attackCheck = 0;
            }
            yield return new WaitForSeconds(0.01f);
        }
        E_Skill_Particle2.SetActive(false);
        transform.position = target.transform.position - target.transform.forward * 5;
        // 모델 활성화
        gameObject.layer = originalLayer;
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
            Eskillbullet projectile = skill.GetComponent<Eskillbullet>();
            if (projectile != null)
            {
                projectile.speed = skillSpeed; // 발사체의 속도 조정
            }

            // 발사 후 스킬을 자동으로 비활성화
            Destroy(skill, DestroyBullet); // 발사체를 5초 후에 파괴 (필요에 따라 조정)
        }

    }
    
    // 데미지 처리 
    public void Damage(Collider hit, float coeff)
    {
        hit.GetComponentInParent<Enemy>().UpdateHP(-PS.power * coeff);
    }

    public bool IsECoolTime()
    {
        return isCoolingDown;
    }

    public bool ComboEActive()
    { 
        return comEActive; 
    }

}
