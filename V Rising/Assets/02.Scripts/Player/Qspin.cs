using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qspin : MonoBehaviour
{
    private Qskill Qskill;
    private Playerstate PState;

    List<Transform> hitObjects = new List<Transform>();

    void Start()
    {
        Qskill = FindObjectOfType<Qskill>();
        PState = GetComponent<Playerstate>();

    }

    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
            if (!hitObjects.Contains(other.transform))      // 히트대상이 리스트에 있으면 통과
            {
                StartCoroutine(hitCoolTime(other.transform));

                Qskill.Damage(other, Qskill.Qdmg);
                PState.UpdateHP(PState.hp_Max * 0.05f);     // 플레이어 회복량

                Debug.Log("Q hit");
            }
        }
    }

    public IEnumerator hitCoolTime(Transform hitObject)
    {
        hitObjects.Add(hitObject);      // 히트대상을 리스트에 추가
        yield return new WaitForSeconds(Qskill.hitFrequency);      // 히트 주기동안 대기
        hitObjects.Remove(hitObject.transform);     // 히트 주기가 끝나면 리스트에서 제외
    }


}
