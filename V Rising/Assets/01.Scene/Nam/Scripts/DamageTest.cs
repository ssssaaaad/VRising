using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public HPSystem healthScript; // 체력 스크립트 참조

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // 체력 20 감소
            healthScript.TakeDamage(20f);
        }
    }
}
