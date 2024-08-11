using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fblood : MonoBehaviour
{
    private Playerstate PState;
    private PlayerManager PManager;
    private PlayerMove playerMove;

    public HP_Scan_Range Scaner;

    public float hp_BloodMax       = 100;
    public float hp_BloodCurrent   = 0;

    public float Healing = 100f;        // 흡혈 회복량
    public float bloodingTime = 2f;     // 흡혈 시간
    public bool dontMove = false;       // 움직이지 마!
    public string noHitPlayer = "NoHitPlayer";      // 피격판정이 없는 레이어

    private bool fActive = false;
    private Collider closeEnemy;
    private Coroutine fCasting;
    private int originalLayer;      // 기존 레이어

    public int cameraShakeTypeIndex = 0;


    public event Action<float, float> OnHealthChanged;


    void Start()
    {
        PState = GetComponent<Playerstate>();
        PManager = GetComponent<PlayerManager>();
        playerMove = GetComponent<PlayerMove>();
        originalLayer = gameObject.layer;

        hp_BloodCurrent = hp_BloodMax;
        OnHealthChanged?.Invoke(hp_BloodCurrent, hp_BloodMax);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(-10f);
        }
    }

    public void F()
    {
        PManager.fblooding = true;


        Debug.Log(Scaner.closeEnemy);

        // F 입력시 가장 가까운 대상 흡혈
        //transform.position = Scaner.closeEnemy.transform.position - Scaner.closeEnemy.transform.forward * 5;

        // 흡혈 캐스팅
        fCasting = StartCoroutine(FCasting());
    }

    public void TakeDamage(float damage)
    {
        hp_BloodCurrent -= damage;
        if (hp_BloodCurrent <= 0)
        {
            hp_BloodCurrent = 0;
            //Die();
        }

        Debug.Log("2222피_현재 : " + hp_BloodCurrent + ", 3323피_맥스 : " + hp_BloodMax);

        UpdateBlood(damage);
    }

    private void UpdateBlood(float dmg)
    {
        if (hp_BloodCurrent > 0)
        {
            hp_BloodCurrent -= dmg;
        }

        if (hp_BloodCurrent > hp_BloodMax)
            hp_BloodCurrent = hp_BloodMax;


        Debug.Log("ㅈㅈㅈ피_현재 : " + hp_BloodCurrent + ", ㅈㅈㅈ피_맥스 : " + hp_BloodMax);

        // HP가 변경되면 이벤트를 호출
        OnHealthChanged?.Invoke(hp_BloodCurrent, hp_BloodMax);

    }

    public IEnumerator FCasting()
    {
        float cast = Time.time + bloodingTime;

        gameObject.layer = LayerMask.NameToLayer(noHitPlayer);

        float check = 0;
        while (check < 1)
        {
            transform.position = Vector3.Lerp(transform.position, Scaner.closeEnemy.transform.position - Scaner.closeEnemy.transform.forward * 3, check);
            check += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        // 흡혈 캐스팅중 플레이어 조작 불가
        dontMove = true;
        transform.forward = Scaner.closeEnemy.transform.forward;
        // 흡혈 캐스팅중 흡혈 대상 이동 불가

        while (cast > Time.time)
        {
            yield return null;
        }

        gameObject.layer = originalLayer;

        dontMove = false;

        // 흡혈 캐스팅 완료시 적 처형 및 체력 회복
        PState.UpdateHP(Healing);       // 회복량 임의설정

        Debug.Log(closeEnemy);
        Damage(Scaner.closeEnemy, 100);     // 적 즉사급 데미지 부여

        PManager.fblooding = false;
    }

    public void FActive(bool canf)
    {
        fActive = canf;
    }

    public void Damage(Collider hit, float coeff)
    {
        hit.GetComponentInParent<Enemy>().UpdateHP(-PState.power * coeff, PManager.transform);

        CameraShakeManager.instance.ShakeSkillCall(cameraShakeTypeIndex);
    }
}
