using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qskill : MonoBehaviour
{
    private PlayerManager PM;

    public float spinDuration = 2f; // 회전 지속시간
    public float cooldownTime = 8f; // 쿨타임 (초)


    private bool isCoolingDown = false;
    private float cooldownEndTime; // 쿨타임 종료 시간
    private Coroutine spinTime;


    void Start()
    {
        PM = GetComponent<PlayerManager>();

    }

    void Update()
    {
        if (PM != null)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isCoolingDown)
            {
                spinTime = StartCoroutine(Spining());
                Debug.Log("Starting Q skill casting.");
            }
        }
    }

    public IEnumerator Spining()
    {
        if (PM != null)
        {
            PM.qskilling = true;
        }
        isCoolingDown = true;
        yield return new WaitForSeconds(spinDuration);



        // 쿨타임 설정
        cooldownEndTime = Time.time + cooldownTime;
        while (Time.time < cooldownEndTime)
        {
            // 쿨타임 동안 대기
            yield return null;
        }

        // 쿨타임 종료
        isCoolingDown = false;
        PM.qskilling = false;

    }

    public void CancelQSkill()
    {

    }

}
