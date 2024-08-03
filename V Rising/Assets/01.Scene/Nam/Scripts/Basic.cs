using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Basic : MonoBehaviour
{
    public Image attackOverlay; // 평타 오버레이 UI 이미지

    private bool isAttacking = false;
    private float attackEndTime;

    void Update()
    {
        if (Input.GetMouseButton(0)) // 좌클릭 시 평타 실행
        {
            StartCoroutine(StartAttack(0.3f, false));
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                StartCoroutine(StartAttack(0.3f, true));
            }
        }
    }

    IEnumerator StartAttack(float attackDuration, bool InputType_ButtonDown)
    {
       
        print("attack");
        isAttacking = true;

        attackOverlay.gameObject.SetActive(true); // 오버레이 활성화
        yield return new WaitForSeconds(attackDuration);

        if (InputType_ButtonDown || !Input.GetMouseButton(0))
        {
            attackOverlay.gameObject.SetActive(false); // 오버레이 비활성화
        }
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
    }

}
