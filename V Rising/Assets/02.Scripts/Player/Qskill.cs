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

    public GameObject Q_Skill_Particle;
    public GameObject Qspin;        // q스킬 히트박스
    public GameObject Qspin_Instance;

    public float Qdmg = 0.35f;

    public float spinDuration = 2f; // 회전 지속시간
    public float cooldownTime = 8f; // 쿨타임 (초)
    public float hitFrequency = 0.3f;   // 다단히트 적용시간

    private bool isCoolingDown = false;
    private float cooldownEndTime; // 쿨타임 종료 시간

    public int cameraShakeTypeIndex = 0;
    private SFXAudioSource spinSound;

    void Start()
    {
        PM = GetComponent<PlayerManager>();
        PS = GetComponent<Playerstate>();
    }


    public void Q()
    {
        if (!isCoolingDown)
        {
            //Q_Skill_Particle.SetActive(false);
            spinTime = StartCoroutine(Spining());
        }
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

        Qspin_Instance = Instantiate(Qspin);    // 히트박스 소환
        Qspin_Instance.transform.position = transform.position;
        Qspin_Instance.transform.forward = transform.forward;
        Qspin_Instance.transform.SetParent(transform);  // 히트박스를 Model의 자식으로 설정

        spinSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.QSkill, transform, Vector3.zero);
        yield return new WaitForSeconds(spinDuration);      // q스킬 지속시간

        Destroy(Qspin_Instance);         // 히트박스 비활성화
        
        Debug.Log("Finishing Q skill casting.");
        //Q_Skill_Particle.SetActive(false);

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

    //private IEnumerator QSkillSound()
    //{
    //    while (PM.qskilling)
    //    {
    //        SoundManager.instance.ActiveOnShotSFXSound()
    //    }
    //}

    public bool IsQCoolTime()
    {
        return isCoolingDown;
    }

    public void CancelQSkill()
    {
        StopCoroutine(Spining());
        if (Qspin_Instance != null)
        {
            Destroy(Qspin_Instance); // 히트박스 비활성화
        }
        if(spinSound != null)
            spinSound.StopSound_FadeOut();
        PM.qskilling = false;
    }

    // hit : 맞은 대상, coeff : 데미지 계수
    public void Damage(Collider hit, float coeff)
    {
        hit.GetComponentInParent<Enemy>().UpdateHP(-PS.power * coeff, PM.transform);

        CameraShakeManager.instance.ShakeSkillCall(cameraShakeTypeIndex);
    }

}
