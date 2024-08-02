using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Qskill : MonoBehaviour
{
    private PlayerManager PM;
    private PlayerMove PlayerMove;

    public float spinDuration = 2f; // 회전 지속시간
    public float cooldownTime = 8f; // 쿨타임 (초)
    public float hitFrequency = 0.2f;   // 다단히트 적용시간
    public Transform hitBox;        // q스킬 히트박스


    private bool isCoolingDown = false;
    private float cooldownEndTime; // 쿨타임 종료 시간
    private Coroutine spinTime;


    void Start()
    {
        PM = GetComponent<PlayerManager>();
        PlayerMove = GetComponent<PlayerMove>();
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
    
    public void ActiveSkill()
    {
       
        StartCoroutine(Spining());
    }

    public IEnumerator Spining()
    {
        PM.qskilling = true;
        isCoolingDown = true;

        hitBox.gameObject.SetActive(true);      // 히트박스 활성화
        yield return new WaitForSeconds(spinDuration);      // q스킬 지속시간
        hitBox.gameObject.SetActive(false);     // 히트박스 비활성화

        Debug.Log("Finishing Q skill casting.");

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

    public void CancelQSkill()
    {
        StopCoroutine(Spining());

        hitBox.gameObject.SetActive(false);     // 히트박스 비활성화

        PM.qskilling = false;
    }

    void OnEnable()
    {
        hitObjects.Clear();
    }

    List<Transform> hitObjects = new List<Transform>();

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!hitObjects.Contains(other.transform))      // 히트대상이 리스트에 있으면 통과
            {
                StartCoroutine(hitCoolTime(other.transform));       
                print("attackMonster");
            }
        }
    }
    public IEnumerator hitCoolTime(Transform hitObject)
    {
        hitObjects.Add(hitObject);      // 히트대상을 리스트에 추가
        yield return new WaitForSeconds(hitFrequency);      // 히트 주기동안 대기
        hitObjects.Remove(hitObject.transform);     // 히트 주기가 끝나면 리스트에서 제외
    }
}
