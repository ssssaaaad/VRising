using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tskill : MonoBehaviour
{
    CharacterController cc;

    private PlayerManager PM;
    private Playerstate PS;
    private PlayerMove PlayerMove;
    private Coroutine ghostTime;
    private IndiControler Indi;

    public GameObject T_Skill_Particle;
    public GameObject Model;
    public GameObject Tboom;

    public float Tdmg = 1.5f;
    public float dashReady = 1f;    // 시전 준비시간
    public float ghostDashing = 0.75f;  // 대쉬 지속시간
    public float afterDashing = 0.3f;   // 대쉬 이후 후딜레이
    public float bustTime = 1.5f;   // 폭발 지연시간
    public float cooldownTime = 20f;    // 쿨타임 (초)
    public float highSpeed = 50f;   // 돌진 속도
    public float normalSpeed = 10f;  // 기본 속도
    public string noHitPlayer = "NoHitPlayer";      // 피격판정이 없는 레이어

    private bool isCoolingDown = false;
    private float cooldownEndTime;  // 쿨타임 종료 시간
    private bool headLock = false;
    private int originalLayer;

    List<Transform> hitObjects = new List<Transform>();

    public GameObject EnemyCollsion;

    public int cameraShakeTypeIndex = 0;

    public int cameraShakeTypeIndexBoom = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        PM = GetComponent<PlayerManager>();
        PS = GetComponent<Playerstate>();
        PlayerMove = GetComponent<PlayerMove>();
        Indi = GetComponent<IndiControler>();
        originalLayer = gameObject.layer;
    }


    public void T()
    {
        Debug.Log("T스킬 시작");
        //T_Skill_Particle.SetActive(true);       // 스킬 이펙트 시작
        ghostTime = StartCoroutine(Ghostdash());
    }

    public IEnumerator Ghostdash()
    {
        PM.tskilling = true;
        isCoolingDown = true;

        Indi.Indi_T();

        PM.animator.SetTrigger("Skill_T_Ready");

        PM.animator.SetTrigger("Skill_T_Ready");
        SFXAudioSource start = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.TSkill_Ready, transform, Vector3.zero);

        yield return new WaitForSeconds(dashReady);         // 시전시간동안 대기 

        start.StopSound_FadeOut();
        PM.animator.SetTrigger("Skill_T_Active");
        SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.TSkill_Active, transform, Vector3.zero);

        headLock = true; // 방향 고정

        Indi.Indi_T_break();

        // 플레이어 Layer를 NoHitPlayer로 변경
        gameObject.layer = LayerMask.NameToLayer(noHitPlayer);

        // EnemyCollision 을 활성화
        EnemyCollsion.SetActive(true);

        // 고정된 방향으로 돌진
        float dashTime = 0f;

        while (dashTime < ghostDashing)
        {
            cc.Move(Model.transform.forward * highSpeed * Time.deltaTime);
            dashTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("T스킬 이동");
        // 데미지만큼 플레이어 회복

        yield return new WaitForSeconds(afterDashing);      // 대쉬 이후 딜레이

        // EnemyCollision 을 비활성화
        EnemyCollsion.SetActive(false);


        //T_Skill_Particle.SetActive(false);          // 스킬 이펙트 종료

        // 기존 레이어로 복귀
        gameObject.layer = originalLayer;

        // 속도 정상화
        PlayerMove.SetSpeed(normalSpeed);
        // 방향 고정 해제
        headLock = false;
        // 돌진 종료
        PM.tskilling = false;
        

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

    public bool IsTCoolTime()
    {
        return isCoolingDown; 
    }

    public void CancelTSkill()
    {
        PM.animator.SetTrigger("CancelSkill");

        StopCoroutine(ghostTime);
        
        PM.tskilling = false;
        Indi.Indi_T_break();
        isCoolingDown = false;

        Debug.Log("Tskill 캔슬");
    }
   

    public IEnumerator hitDelayTime(Transform hitObject)
    {
        CameraShakeManager.instance.ShakeSkillCall(cameraShakeTypeIndex);

        yield return new WaitForSeconds(bustTime);      // 폭발 지연시간동안 대기

        GameObject tBoom = Instantiate(Tboom);
        tBoom.transform.position = hitObject.position;
        Destroy(tBoom, 0.2f);
        print("Boom");      // 폭발 (Tboom 소환)
    }

    public bool HeadLock()
    {
        return headLock;
    }

    // hit : 맞은 대상, coeff : 데미지 계수
    public void Damage(Collider hit, float coeff)
    {
        hit.GetComponentInParent<Enemy>().UpdateHP(-PS.power * coeff, PM.transform);

        CameraShakeManager.instance.ShakeSkillCall(cameraShakeTypeIndexBoom);
    }
}
