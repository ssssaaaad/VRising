using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskill : MonoBehaviour
{
    PlayerManager PM;

    public float skillSpeed = 20f; // 발사체의 속도
    public float castTime = 1f; // 시전 시간 (초)
    public float cooldownTime = 8f; // 쿨타임 (초)
    public float reUseTime = 1f;    // 스킬 재활성화 시간

    private bool isHit = false;     // 투사체 적중 여부 확인
    private bool isCoolingDown = false; // 쿨타임 여부 확인
    private float cooldownEndTime; // 쿨타임 종료 시간


    void Start()
    {
        PM = GetComponent<PlayerManager>();
    }

    void Update()
    {
        // e 클릭시 사용가능여부 확인
        // 짧은 케스팅
        // 이후 투사체 발사
        // 투사체 적중여부 확인
        // 투사체 적중시 1초 후 스킬 재활성화
        // 재활성화된 스킬 사용시 투사체를 맞는 적에게 이동(?)
    }
}
