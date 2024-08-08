using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fblood : MonoBehaviour
{
    private Playerstate PState;
    private PlayerManager PManager;
    private PlayerMove playerMove;

    public HP_Scan_Range Scaner;

    private bool fActive = false;
    private Enemy closeEnemy;

    void Start()
    {
        PState = GetComponent<Playerstate>();
        PManager = GetComponent<PlayerManager>();
        playerMove = GetComponent<PlayerMove>();
    }

    public void F()
    {
        if (!fActive)
            return;
        // F 입력시 가장 가까운 대상 흡혈
        transform.position = Scaner.closeEnemy.position - Scaner.closeEnemy.forward * 5;
        // 흡혈중 플레이어 조작 불가
        
        // 흡혈중 흡혈 대상 이동 불가
        // 적 처형 및 체력 회복

    }

    public void FActive(bool canf)
    {
        fActive = canf;
    }
}
