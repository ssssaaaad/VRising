using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskill : MonoBehaviour
{
    private PlayerManager PM;
    private Coroutine castingCoroutine;
    private PlayerMove playerMove;

    public GameObject skillPrefab; // 발사할 스킬(발사체) 프리팹
    public Transform firePoint; // 스킬이 발사될 위치
    public SkillUI skillUI;
    public float skillSpeed = 20f; // 발사체의 속도
    public float castTime = 1f; // 시전 시간 (초)
    public float cooldownTime = 8f; // 쿨타임 (초)
    public float slowSpeed = 0.5f; // 시전 시 캐릭터 속도 감소
    public float DestroyBullet = 1f; // 검기이 부셔지는 시간

    private float playerSpeed; // 정상 캐릭터 속도
    private float cooldownEndTime; // 쿨타임 종료 시간
    private bool isCoolingDown = false; // 쿨타임 여부 확인
    private bool isHit = false;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        PM = GetComponent<PlayerManager>();
        playerSpeed = playerMove.playerSpeed;
    }

    void Update()
    {
        
    }

    public void E()
    {
        Debug.Log("Eskill 시전");
        castingCoroutine = StartCoroutine(CastSkill());
    }

    public IEnumerator CastSkill()
    {
        PM.eskilling = true;

        // 이동속도 감소
        // 캐스팅 시간동안 대기
        // 투사체 발사


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

    public void ComboE()
    {
        // 재시전 E 
    }

    void ActivateSkill()
    {
        
        
    }


    public bool IsECoolTime()
    {
        return isCoolingDown;
    }

    public void CancelTSkill()
    {

    }
}
