using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskill : MonoBehaviour
{
    private PlayerManager PM;

    public float cooldownTime = 10f;    // 쿨타임 (초)
    public float reUseTime = 1f;        // 연속사용 지연시간
    public float reUseTimer = 1f;       // 연속사용 활성화시간

    private bool isCoolingDown = false;
    private bool isHit = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void E()
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
